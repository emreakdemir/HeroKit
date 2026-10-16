using System.Buffers;

namespace HeroKit.FileTypes;

public static partial class FileTypes
{
    // Server-side and Windows script extensions that are not built-in types, for the extension checks of uploads.
    private static readonly HashSet<string> ExtraDangerousExtensions = new(StringComparer.Ordinal)
    {
        "asp", "aspx", "ascx", "ashx", "asmx", "asa", "axd", "cer", "config", "jsp", "jspx", "cgi", "pl", "py", "pyc", "rb",
        "jse", "msc", "reg", "inf", "pif", "gadget", "application", "appref-ms", "vb", "ws", "url", "desktop", "iqy", "slk",
    };

    /// <summary>
    /// Checks whether an uploaded file is safe to accept, following the OWASP file upload advice.
    /// </summary>
    /// <remarks>
    /// <para>The checks run in this order: file name, program or script extension (also hidden as in "a.php.jpg"), empty content,
    /// program or script content, known extension, detected content, content fits the extension, <see cref="UploadPolicy.AllowedTypes"/>,
    /// active content in SVG, then <see cref="UploadPolicy.Rules"/>.</para>
    /// <para>This overload scans an SVG only as far as <paramref name="content"/> goes; pass the whole file or use the stream overload.
    /// The check does not replace size limits, generated file names, re-encoding images or virus scanning.</para>
    /// </remarks>
    /// <param name="content">The file content, or at least its first <see cref="HeaderSize"/> bytes.</param>
    /// <param name="fileName">The file name as uploaded.</param>
    /// <param name="policy">The policy.</param>
    /// <returns>The result with the reason of a rejection.</returns>
    /// <example>
    /// <code>
    /// FileTypes.CheckUpload(pngBytes, "photo.png").IsSafe;        // true
    /// FileTypes.CheckUpload(exeBytes, "photo.png").Rejection;     // DangerousContent
    /// FileTypes.CheckUpload(pngBytes, "photo.php.png").Rejection; // DoubleExtension
    /// </code>
    /// </example>
    public static UploadCheckResult CheckUpload(ReadOnlySpan<byte> content, string fileName, UploadPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(policy);
        UploadCheckResult result = CheckBeforeSvg(content, fileName, policy, out FileType? detected);
        if (!result.IsSafe)
            return result;

        if (detected is not null && detected.IsA(Svg) && HasActiveSvgContent(content))
            return Reject(UploadRejection.ActiveContent, detected, "The SVG contains script, event handlers, embedded HTML, entity declarations or external references.");

        return RunRules(content[..Math.Min(content.Length, HeaderSize)], fileName, policy, detected);
    }

    /// <summary>
    /// Checks whether an uploaded file is safe to accept. Reads at most <see cref="HeaderSize"/> bytes, or the whole file up to
    /// <see cref="UploadPolicy.MaxSvgScanLength"/> for SVG, and restores the position of a seekable stream.
    /// </summary>
    /// <param name="stream">The stream, positioned at the start of the file.</param>
    /// <param name="fileName">The file name as uploaded.</param>
    /// <param name="policy">The policy.</param>
    /// <returns>The result with the reason of a rejection.</returns>
    public static UploadCheckResult CheckUpload(Stream stream, string fileName, UploadPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(policy);

        long start = stream.CanSeek ? stream.Position : 0;
        byte[] buffer = ArrayPool<byte>.Shared.Rent(HeaderSize);
        byte[]? svgBuffer = null;
        try
        {
            int read = stream.ReadAtLeast(buffer.AsSpan(0, HeaderSize), HeaderSize, throwOnEndOfStream: false);
            ReadOnlySpan<byte> header = buffer.AsSpan(0, read);

            UploadCheckResult result = CheckBeforeSvg(header, fileName, policy, out FileType? detected);
            if (!result.IsSafe)
                return result;

            if (detected is not null && detected.IsA(Svg))
            {
                // Read the rest of the SVG, one byte past the limit to notice a larger file.
                int limit = Math.Max(policy.MaxSvgScanLength, read);
                svgBuffer = ArrayPool<byte>.Shared.Rent(limit + 1);
                header.CopyTo(svgBuffer);
                int total = read + stream.ReadAtLeast(svgBuffer.AsSpan(read, limit + 1 - read), limit + 1 - read, throwOnEndOfStream: false);
                if (total > limit)
                    return Reject(UploadRejection.ActiveContent, detected, "The SVG is larger than the scan limit.");

                if (HasActiveSvgContent(svgBuffer.AsSpan(0, total)))
                    return Reject(UploadRejection.ActiveContent, detected, "The SVG contains script, event handlers, embedded HTML, entity declarations or external references.");
            }

            return RunRules(header, fileName, policy, detected);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
            if (svgBuffer is not null)
                ArrayPool<byte>.Shared.Return(svgBuffer);

            if (stream.CanSeek)
                stream.Position = start;
        }
    }

    /// <summary>
    /// Checks whether an uploaded file is safe to accept with <see cref="UploadPolicy.Default"/>.
    /// See <see cref="CheckUpload(ReadOnlySpan{byte}, string, UploadPolicy)"/>.
    /// </summary>
    /// <param name="content">The file content, or at least its first <see cref="HeaderSize"/> bytes.</param>
    /// <param name="fileName">The file name as uploaded.</param>
    /// <returns>The result with the reason of a rejection.</returns>
    public static UploadCheckResult CheckUpload(ReadOnlySpan<byte> content, string fileName) => CheckUpload(content, fileName, UploadPolicy.Default);

    /// <summary>
    /// Checks whether an uploaded file is safe to accept with <see cref="UploadPolicy.Default"/>.
    /// See <see cref="CheckUpload(Stream, string, UploadPolicy)"/>.
    /// </summary>
    /// <param name="stream">The stream, positioned at the start of the file.</param>
    /// <param name="fileName">The file name as uploaded.</param>
    /// <returns>The result with the reason of a rejection.</returns>
    public static UploadCheckResult CheckUpload(Stream stream, string fileName) => CheckUpload(stream, fileName, UploadPolicy.Default);

    private static UploadCheckResult CheckBeforeSvg(ReadOnlySpan<byte> content, string fileName, UploadPolicy policy, out FileType? detected)
    {
        detected = null;
        if (!IsValidFileName(fileName, policy.MaxFileNameLength))
            return Reject(UploadRejection.InvalidFileName, null, "The file name is not allowed.");

        string[] parts = fileName.Split('.');
        string? extension = parts.Length > 1 ? parts[^1].ToLowerInvariant() : null;
        if (extension is not null && IsDangerousExtension(extension))
            return Reject(UploadRejection.DangerousExtension, null, $"Files with the extension '.{extension}' are not allowed.");

        for (int i = 1; i < parts.Length - 1; i++)
        {
            if (IsDangerousExtension(parts[i].ToLowerInvariant()))
                return Reject(UploadRejection.DoubleExtension, null, $"The file name hides the extension '.{parts[i]}'.");
        }

        if (content.IsEmpty)
            return Reject(UploadRejection.EmptyContent, null, "The file is empty.");

        ReadOnlySpan<byte> header = content[..Math.Min(content.Length, HeaderSize)];
        foreach (FileType match in DetectAll(header))
        {
            if (match.IsExecutable)
                return Reject(UploadRejection.DangerousContent, match, $"The content is a {match.Name}.");
        }

        FileType? expected = extension is null ? null : FromExtension(extension);
        if (expected is null)
            return Reject(UploadRejection.UnknownExtension, null, "The file extension is missing or unknown.");

        detected = Detect(header);
        if (detected is null)
        {
            if (!policy.AllowUnknownContent)
                return Reject(UploadRejection.UnknownContent, null, "The content type could not be detected.");
        }
        else if (!AreCompatible(detected, expected))
        {
            return Reject(UploadRejection.ExtensionMismatch, detected, $"The content is a {detected.Name}, not a {expected.Name}.");
        }

        if (policy.AllowedTypes is not null && !policy.AllowedTypes.Contains(expected))
            return Reject(UploadRejection.NotAllowed, detected, $"{expected.Name} files are not allowed.");

        return new UploadCheckResult(UploadRejection.None, detected, null);
    }

    private static UploadCheckResult RunRules(ReadOnlySpan<byte> header, string fileName, UploadPolicy policy, FileType? detected)
    {
        foreach (IUploadRule rule in policy.Rules)
        {
            string? message = rule.Validate(header, fileName, detected);
            if (message is not null)
                return Reject(UploadRejection.CustomRule, detected, message);
        }

        return new UploadCheckResult(UploadRejection.None, detected, null);
    }

    private static UploadCheckResult Reject(UploadRejection rejection, FileType? detected, string message) => new(rejection, detected, message);

    private static bool IsDangerousExtension(string extension) =>
        ExtraDangerousExtensions.Contains(extension) || FromExtension(extension) is { IsExecutable: true };

    private static bool IsValidFileName(string? fileName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(fileName) || fileName.Length > maxLength)
            return false;

        if (fileName[0] is '.' or ' ' || fileName[^1] is '.' or ' ')
            return false;

        foreach (char c in fileName)
        {
            if (c < 0x20 || c == 0x7F || c is '/' or '\\' or ':' or '*' or '?' or '"' or '<' or '>' or '|')
                return false;
        }

        return true;
    }

    // Script, event handlers, embedded HTML, entity declarations (XXE) and external references, compared without case.
    // A DOCTYPE alone is allowed because editors write one; entity declarations inside it are not.
    private static readonly byte[][] SvgMarkers =
    [
        .. new[]
        {
            "<script", "javascript:", "vbscript:", "data:text/html", "<foreignobject", "<iframe", "<embed", "<object",
            "<!entity", "href=\"http", "href='http", "href=\"//", "href='//", "@import",
        }.Select(System.Text.Encoding.ASCII.GetBytes),
    ];

    private static bool HasActiveSvgContent(ReadOnlySpan<byte> svg)
    {
        foreach (byte[] marker in SvgMarkers)
        {
            if (Sniffers.IndexOfIgnoreCase(svg, marker) >= 0)
                return true;
        }

        return HasEventHandler(svg);
    }

    // An attribute such as onload= or onclick = after whitespace.
    private static bool HasEventHandler(ReadOnlySpan<byte> svg)
    {
        for (int i = 1; i + 3 < svg.Length; i++)
        {
            if (svg[i - 1] is not ((byte)' ' or (byte)'\t' or (byte)'\r' or (byte)'\n' or (byte)'/') || !Sniffers.StartsWithIgnoreCase(svg[i..], "on"u8))
                continue;

            int j = i + 2;
            int letters = 0;
            while (j < svg.Length && (svg[j] | 0x20) is >= (byte)'a' and <= (byte)'z')
            {
                j++;
                letters++;
            }

            while (j < svg.Length && svg[j] is (byte)' ' or (byte)'\t' or (byte)'\r' or (byte)'\n')
                j++;

            if (letters > 0 && j < svg.Length && svg[j] == '=')
                return true;
        }

        return false;
    }
}
