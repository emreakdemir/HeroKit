using System.Buffers;

namespace HeroKit.FileTypes;

/// <summary>
/// Built-in file types, a registry for your own types, and detection by content, extension and MIME type.
/// </summary>
/// <remarks>
/// <para>Detection reads magic bytes from the start of a file. It is a first check, not a proof: a file can be crafted to match
/// a type. For uploads, also limit the size, store files under generated names and re-encode or scan them when it matters.</para>
/// <para>Types you register with <see cref="Register"/> are checked before the built-in types, and their extensions and MIME types win.</para>
/// </remarks>
/// <example>
/// <code>
/// using var stream = File.OpenRead("photo.bin");
/// FileTypes.Detect(stream)?.MimeType;         // "image/png"
/// FileTypes.FromExtension("report.docx")?.Name; // "Word document"
/// FileTypes.GetMimeType("archive.tar.gz");    // "application/gzip"
/// </code>
/// </example>
public static partial class FileTypes
{
    /// <summary>
    /// The number of bytes read from the start of a stream for detection: 16 KB.
    /// </summary>
    public const int HeaderSize = 16 * 1024;

    /// <summary>
    /// The MIME type for unknown binary content: "application/octet-stream".
    /// </summary>
    public const string DefaultMimeType = "application/octet-stream";

    private static readonly object RegistryLock = new();
    private static Registry _registry = null!;

    static FileTypes() => _registry = new Registry([], BuiltIn);

    /// <summary>
    /// Gets all types: registered types first (newest first), then the built-in types in detection order.
    /// </summary>
    public static IReadOnlyList<FileType> All => _registry.Ordered;

    /// <summary>
    /// Registers a file type. It is checked before the built-in types and before types registered earlier.
    /// </summary>
    /// <param name="fileType">The type.</param>
    public static void Register(FileType fileType)
    {
        ArgumentNullException.ThrowIfNull(fileType);
        lock (RegistryLock)
        {
            _registry = new Registry([fileType, .. _registry.Custom], BuiltIn);
        }
    }

    /// <summary>
    /// Detects the type of a file from its first bytes.
    /// </summary>
    /// <param name="header">The first bytes of the file. 16 KB (<see cref="HeaderSize"/>) is enough for every built-in type.</param>
    /// <returns>The first matching type, or null if no type matches.</returns>
    public static FileType? Detect(ReadOnlySpan<byte> header)
    {
        if (header.IsEmpty)
            return null;

        foreach (FileType type in _registry.Detectable)
        {
            if (type.IsMatch(header))
                return type;
        }

        return null;
    }

    /// <summary>
    /// Returns every type whose detectors match the first bytes of a file, for example both DOCX and ZIP.
    /// </summary>
    /// <param name="header">The first bytes of the file.</param>
    /// <returns>The matching types in detection order; empty if none match.</returns>
    public static IReadOnlyList<FileType> DetectAll(ReadOnlySpan<byte> header)
    {
        var result = new List<FileType>();
        if (header.IsEmpty)
            return result;

        foreach (FileType type in _registry.Detectable)
        {
            if (type.IsMatch(header))
                result.Add(type);
        }

        return result;
    }

    /// <summary>
    /// Detects the type of a file from the start of a stream. Reads at most <see cref="HeaderSize"/> bytes and restores the position of a seekable stream.
    /// </summary>
    /// <param name="stream">The stream, positioned at the start of the file.</param>
    /// <returns>The first matching type, or null.</returns>
    public static FileType? Detect(Stream stream)
    {
        byte[] buffer = ArrayPool<byte>.Shared.Rent(HeaderSize);
        try
        {
            int read = ReadHeader(stream, buffer);
            return Detect(buffer.AsSpan(0, read));
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    /// <summary>
    /// Detects the type of a file from the start of a stream asynchronously. Reads at most <see cref="HeaderSize"/> bytes and restores the position of a seekable stream.
    /// </summary>
    /// <param name="stream">The stream, positioned at the start of the file.</param>
    /// <param name="cancellationToken">A token to cancel the read.</param>
    /// <returns>The first matching type, or null.</returns>
    public static async ValueTask<FileType?> DetectAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        byte[] buffer = ArrayPool<byte>.Shared.Rent(HeaderSize);
        try
        {
            int read = await ReadHeaderAsync(stream, buffer, cancellationToken).ConfigureAwait(false);
            return Detect(buffer.AsSpan(0, read));
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    /// <summary>
    /// Finds a type by the extension of a file name or by an extension, ignoring case.
    /// </summary>
    /// <param name="fileNameOrExtension">A file name or path ("photo.JPG"), an extension with a dot (".jpg") or without one ("jpg").</param>
    /// <returns>The type, or null if the extension is unknown or missing.</returns>
    public static FileType? FromExtension(string? fileNameOrExtension)
    {
        string? extension = GetExtension(fileNameOrExtension);
        return extension is not null && _registry.ByExtension.TryGetValue(extension, out FileType? type) ? type : null;
    }

    /// <summary>
    /// Finds a type by its MIME type, ignoring case and parameters such as "; charset=utf-8".
    /// </summary>
    /// <param name="mimeType">The MIME type, for example "image/jpeg" or "image/x-icon".</param>
    /// <returns>The type, or null if the MIME type is unknown.</returns>
    public static FileType? FromMimeType(string? mimeType)
    {
        if (string.IsNullOrWhiteSpace(mimeType))
            return null;

        int separator = mimeType.IndexOf(';');
        string key = (separator >= 0 ? mimeType[..separator] : mimeType).Trim().ToLowerInvariant();
        return _registry.ByMimeType.TryGetValue(key, out FileType? type) ? type : null;
    }

    /// <summary>
    /// Returns the MIME type for a file name, for example for a Content-Type header.
    /// </summary>
    /// <param name="fileNameOrExtension">A file name, path or extension.</param>
    /// <returns>The MIME type, or <see cref="DefaultMimeType"/> if the extension is unknown.</returns>
    public static string GetMimeType(string? fileNameOrExtension) => FromExtension(fileNameOrExtension)?.MimeType ?? DefaultMimeType;

    /// <summary>
    /// Checks whether the content of a file fits the extension of its name.
    /// </summary>
    /// <remarks>
    /// The content fits when the detected type is the extension's type, or one is built on the other through <see cref="FileType.Parent"/>:
    /// a ".docx" whose entries are beyond the header is accepted as ZIP, and a ".csv" is accepted as plain text.
    /// </remarks>
    /// <param name="header">The first bytes of the file.</param>
    /// <param name="fileName">The file name or extension.</param>
    /// <returns>True if both are known and fit; false if either is unknown or they differ.</returns>
    /// <example>
    /// <code>
    /// FileTypes.ContentMatchesExtension(pngBytes, "photo.png"); // true
    /// FileTypes.ContentMatchesExtension(exeBytes, "photo.png"); // false
    /// </code>
    /// </example>
    public static bool ContentMatchesExtension(ReadOnlySpan<byte> header, string? fileName)
    {
        FileType? expected = FromExtension(fileName);
        FileType? detected = Detect(header);
        return expected is not null && detected is not null && AreCompatible(detected, expected);
    }

    /// <summary>
    /// Checks whether the start of a stream fits the extension of a file name. Reads at most <see cref="HeaderSize"/> bytes and restores the position of a seekable stream.
    /// </summary>
    /// <param name="stream">The stream, positioned at the start of the file.</param>
    /// <param name="fileName">The file name or extension.</param>
    /// <returns>True if both are known and fit.</returns>
    public static bool ContentMatchesExtension(Stream stream, string? fileName)
    {
        byte[] buffer = ArrayPool<byte>.Shared.Rent(HeaderSize);
        try
        {
            int read = ReadHeader(stream, buffer);
            return ContentMatchesExtension(buffer.AsSpan(0, read), fileName);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    internal static bool AreCompatible(FileType detected, FileType expected) =>
        detected.IsA(expected) || expected.IsA(detected) || string.Equals(detected.MimeType, expected.MimeType, StringComparison.Ordinal);

    // The text after the last dot of the file name; a value without a dot is taken as an extension.
    internal static string? GetExtension(string? fileNameOrExtension)
    {
        if (string.IsNullOrWhiteSpace(fileNameOrExtension))
            return null;

        string value = fileNameOrExtension.Trim();
        int separator = value.LastIndexOfAny(['/', '\\']);
        string name = separator >= 0 ? value[(separator + 1)..] : value;
        int dot = name.LastIndexOf('.');
        string extension = dot >= 0 ? name[(dot + 1)..] : name;
        return extension.Length == 0 ? null : extension.ToLowerInvariant();
    }

    internal static int ReadHeader(Stream stream, byte[] buffer)
    {
        ArgumentNullException.ThrowIfNull(stream);
        long start = stream.CanSeek ? stream.Position : 0;
        int read = stream.ReadAtLeast(buffer.AsSpan(0, HeaderSize), HeaderSize, throwOnEndOfStream: false);
        if (stream.CanSeek)
            stream.Position = start;

        return read;
    }

    internal static async ValueTask<int> ReadHeaderAsync(Stream stream, byte[] buffer, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stream);
        long start = stream.CanSeek ? stream.Position : 0;
        int read = await stream.ReadAtLeastAsync(buffer.AsMemory(0, HeaderSize), HeaderSize, throwOnEndOfStream: false, cancellationToken).ConfigureAwait(false);
        if (stream.CanSeek)
            stream.Position = start;

        return read;
    }

    private sealed class Registry
    {
        public Registry(FileType[] custom, FileType[] builtIn)
        {
            Custom = custom;
            Ordered = [.. custom, .. builtIn];
            Detectable = [.. Ordered.Where(t => t.Detectors.Count > 0)];

            foreach (FileType type in Ordered)
            {
                foreach (string extension in type.Extensions)
                    ByExtension.TryAdd(extension, type);

                ByMimeType.TryAdd(type.MimeType, type);
                foreach (string other in type.OtherMimeTypes)
                    ByMimeType.TryAdd(other.Trim().ToLowerInvariant(), type);
            }
        }

        public FileType[] Custom { get; }

        public FileType[] Ordered { get; }

        public FileType[] Detectable { get; }

        public Dictionary<string, FileType> ByExtension { get; } = new(StringComparer.Ordinal);

        public Dictionary<string, FileType> ByMimeType { get; } = new(StringComparer.Ordinal);
    }
}
