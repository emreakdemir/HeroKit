namespace HeroKit.FileTypes;

/// <summary>
/// The built-in file types and shortcuts to the shared <see cref="Default"/> registry and to <see cref="UploadValidator"/>.
/// </summary>
/// <remarks>
/// The static methods use <see cref="Default"/>. Inject a <see cref="FileTypeRegistry"/> or an <see cref="UploadValidator"/> instead
/// when you want your own set of types or depend on them through constructor injection.
/// </remarks>
/// <example>
/// <code>
/// using var stream = File.OpenRead("photo.bin");
/// FileTypes.Detect(stream)?.MimeType;           // "image/png"
/// FileTypes.FromExtension("report.docx")?.Name; // "Word document"
/// FileTypes.GetMimeType("archive.tar.gz");      // "application/gzip"
/// FileTypes.CheckUpload(stream, "photo.png").IsSafe;
/// </code>
/// </example>
public static partial class FileTypes
{
    /// <summary>The number of bytes read from the start of a stream for detection: 16 KB.</summary>
    public const int HeaderSize = FileTypeRegistry.HeaderSize;

    /// <summary>The MIME type for unknown binary content: "application/octet-stream".</summary>
    public const string DefaultMimeType = FileTypeRegistry.DefaultMimeType;

    private static readonly UploadValidator DefaultValidator;

    // Runs after every static field of both partial files is initialized, so the built-in list is complete.
    static FileTypes()
    {
        Default = new FileTypeRegistry(BuiltInTypes);
        DefaultValidator = new UploadValidator(UploadPolicy.Default, Default);
    }

    /// <summary>Gets the built-in types in detection order: specific types before the types they are built on, plain text last.</summary>
    public static IReadOnlyList<FileType> BuiltIn => BuiltInTypes;

    /// <summary>Gets the shared registry with the built-in types and the types added with <see cref="Register"/>.</summary>
    public static FileTypeRegistry Default { get; }

    /// <summary>Gets all types of <see cref="Default"/>. See <see cref="FileTypeRegistry.All"/>.</summary>
    public static IReadOnlyList<FileType> All => Default.All;

    /// <summary>Registers a type in <see cref="Default"/>. See <see cref="FileTypeRegistry.Register"/>.</summary>
    /// <param name="fileType">The type.</param>
    public static void Register(FileType fileType) => Default.Register(fileType);

    /// <summary>Detects the type of a file from its first bytes. See <see cref="FileTypeRegistry.Detect(ReadOnlySpan{byte})"/>.</summary>
    /// <param name="header">The first bytes of the file.</param>
    /// <returns>The first matching type, or null.</returns>
    public static FileType? Detect(ReadOnlySpan<byte> header) => Default.Detect(header);

    /// <summary>Returns every matching type. See <see cref="FileTypeRegistry.DetectAll"/>.</summary>
    /// <param name="header">The first bytes of the file.</param>
    /// <returns>The matching types in detection order.</returns>
    public static IReadOnlyList<FileType> DetectAll(ReadOnlySpan<byte> header) => Default.DetectAll(header);

    /// <summary>Detects the type of a file from a stream. See <see cref="FileTypeRegistry.Detect(Stream)"/>.</summary>
    /// <param name="stream">The stream, positioned at the start of the file.</param>
    /// <returns>The first matching type, or null.</returns>
    public static FileType? Detect(Stream stream) => Default.Detect(stream);

    /// <summary>Detects the type of a file from a stream asynchronously. See <see cref="FileTypeRegistry.DetectAsync"/>.</summary>
    /// <param name="stream">The stream, positioned at the start of the file.</param>
    /// <param name="cancellationToken">A token to cancel the read.</param>
    /// <returns>The first matching type, or null.</returns>
    public static ValueTask<FileType?> DetectAsync(Stream stream, CancellationToken cancellationToken = default) => Default.DetectAsync(stream, cancellationToken);

    /// <summary>Finds a type by extension. See <see cref="FileTypeRegistry.FromExtension"/>.</summary>
    /// <param name="fileNameOrExtension">A file name, path or extension.</param>
    /// <returns>The type, or null.</returns>
    public static FileType? FromExtension(string? fileNameOrExtension) => Default.FromExtension(fileNameOrExtension);

    /// <summary>Finds a type by MIME type. See <see cref="FileTypeRegistry.FromMimeType"/>.</summary>
    /// <param name="mimeType">The MIME type.</param>
    /// <returns>The type, or null.</returns>
    public static FileType? FromMimeType(string? mimeType) => Default.FromMimeType(mimeType);

    /// <summary>Returns the MIME type for a file name. See <see cref="FileTypeRegistry.GetMimeType"/>.</summary>
    /// <param name="fileNameOrExtension">A file name, path or extension.</param>
    /// <returns>The MIME type, or <see cref="DefaultMimeType"/>.</returns>
    public static string GetMimeType(string? fileNameOrExtension) => Default.GetMimeType(fileNameOrExtension);

    /// <summary>Checks whether content fits the extension. See <see cref="FileTypeRegistry.ContentMatchesExtension(ReadOnlySpan{byte}, string)"/>.</summary>
    /// <param name="header">The first bytes of the file.</param>
    /// <param name="fileName">The file name or extension.</param>
    /// <returns>True if both are known and fit.</returns>
    public static bool ContentMatchesExtension(ReadOnlySpan<byte> header, string? fileName) => Default.ContentMatchesExtension(header, fileName);

    /// <summary>Checks whether a stream fits the extension. See <see cref="FileTypeRegistry.ContentMatchesExtension(Stream, string)"/>.</summary>
    /// <param name="stream">The stream, positioned at the start of the file.</param>
    /// <param name="fileName">The file name or extension.</param>
    /// <returns>True if both are known and fit.</returns>
    public static bool ContentMatchesExtension(Stream stream, string? fileName) => Default.ContentMatchesExtension(stream, fileName);

    /// <summary>Checks an upload with <see cref="UploadPolicy.Default"/>. See <see cref="UploadValidator.Validate(ReadOnlySpan{byte}, string)"/>.</summary>
    /// <param name="content">The file content, or at least its first <see cref="HeaderSize"/> bytes.</param>
    /// <param name="fileName">The file name as uploaded.</param>
    /// <returns>The result with the reason of a rejection.</returns>
    public static UploadCheckResult CheckUpload(ReadOnlySpan<byte> content, string fileName) => DefaultValidator.Validate(content, fileName);

    /// <summary>Checks an upload with a policy. See <see cref="UploadValidator.Validate(ReadOnlySpan{byte}, string)"/>.</summary>
    /// <param name="content">The file content, or at least its first <see cref="HeaderSize"/> bytes.</param>
    /// <param name="fileName">The file name as uploaded.</param>
    /// <param name="policy">The policy.</param>
    /// <returns>The result with the reason of a rejection.</returns>
    public static UploadCheckResult CheckUpload(ReadOnlySpan<byte> content, string fileName, UploadPolicy policy) =>
        new UploadValidator(policy, Default).Validate(content, fileName);

    /// <summary>Checks an upload with <see cref="UploadPolicy.Default"/>. See <see cref="UploadValidator.Validate(Stream, string)"/>.</summary>
    /// <param name="stream">The stream, positioned at the start of the file.</param>
    /// <param name="fileName">The file name as uploaded.</param>
    /// <returns>The result with the reason of a rejection.</returns>
    public static UploadCheckResult CheckUpload(Stream stream, string fileName) => DefaultValidator.Validate(stream, fileName);

    /// <summary>Checks an upload with a policy. See <see cref="UploadValidator.Validate(Stream, string)"/>.</summary>
    /// <param name="stream">The stream, positioned at the start of the file.</param>
    /// <param name="fileName">The file name as uploaded.</param>
    /// <param name="policy">The policy.</param>
    /// <returns>The result with the reason of a rejection.</returns>
    public static UploadCheckResult CheckUpload(Stream stream, string fileName, UploadPolicy policy) =>
        new UploadValidator(policy, Default).Validate(stream, fileName);

    private static MagicNumber M(string pattern, int offset = 0) => new(pattern, offset);

    private static MagicNumber T(string text, int offset = 0) => MagicNumber.FromText(text, offset);

    private static DelegateDetector D(DelegateDetector.Match match) => new(match);
}
