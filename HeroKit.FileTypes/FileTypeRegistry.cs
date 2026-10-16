namespace HeroKit.FileTypes;

/// <summary>
/// A set of file types with detection by content and lookup by extension and MIME type.
/// </summary>
/// <remarks>
/// <para><see cref="FileTypes.Default"/> holds the built-in types and is what the static <see cref="FileTypes"/> methods use.
/// Create your own instance to keep custom types out of the shared registry, for example per tenant, in tests or through dependency injection.</para>
/// <para>The registry is thread-safe. <see cref="Register"/> replaces an internal snapshot, so readers never see a half-updated state.</para>
/// <para>Detection reads magic bytes from the start of a file. It is a first check, not a proof: a file can be crafted to match a type.</para>
/// </remarks>
/// <example>
/// <code>
/// var registry = new FileTypeRegistry(); // the built-in types
/// registry.Register(new FileType("Sketch document", "application/x-sketch", FileTypeCategory.Document, "sketch")
/// {
///     Detectors = [new ZipEntryDetector("document.json")],
///     Parent = FileTypes.Zip,
/// });
/// services.AddSingleton(registry);
/// </code>
/// </example>
public sealed class FileTypeRegistry
{
    /// <summary>
    /// The number of bytes read from the start of a stream for detection: 16 KB.
    /// </summary>
    public const int HeaderSize = 16 * 1024;

    /// <summary>
    /// The MIME type for unknown binary content: "application/octet-stream".
    /// </summary>
    public const string DefaultMimeType = "application/octet-stream";

    private readonly object _writeLock = new();
    private readonly IReadOnlyList<FileType> _baseTypes;
    private Snapshot _snapshot;

    /// <summary>
    /// Creates a registry with the built-in types (<see cref="FileTypes.BuiltIn"/>).
    /// </summary>
    public FileTypeRegistry()
        : this(FileTypes.BuiltIn)
    {
    }

    /// <summary>
    /// Creates a registry with only the given types, in detection order: put specific types before the types they are built on.
    /// </summary>
    /// <param name="types">The types.</param>
    public FileTypeRegistry(IEnumerable<FileType> types)
    {
        ArgumentNullException.ThrowIfNull(types);
        _baseTypes = [.. types];
        _snapshot = new Snapshot([], _baseTypes);
    }

    /// <summary>
    /// Gets all types: registered types first (newest first), then the types given to the constructor.
    /// </summary>
    public IReadOnlyList<FileType> All => _snapshot.Ordered;

    /// <summary>
    /// Registers a file type. It is checked before the other types, and its extensions and MIME types win.
    /// </summary>
    /// <param name="fileType">The type.</param>
    public void Register(FileType fileType)
    {
        ArgumentNullException.ThrowIfNull(fileType);
        lock (_writeLock)
        {
            _snapshot = new Snapshot([fileType, .. _snapshot.Registered], _baseTypes);
        }
    }

    /// <summary>
    /// Detects the type of a file from its first bytes.
    /// </summary>
    /// <param name="header">The first bytes of the file. <see cref="HeaderSize"/> bytes are enough for every built-in type; a longer span lets the ZIP detectors see more entries.</param>
    /// <returns>The first matching type, or null if no type matches.</returns>
    public FileType? Detect(ReadOnlySpan<byte> header)
    {
        if (header.IsEmpty)
            return null;

        foreach (FileType type in _snapshot.Detectable)
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
    public IReadOnlyList<FileType> DetectAll(ReadOnlySpan<byte> header)
    {
        var result = new List<FileType>();
        if (header.IsEmpty)
            return result;

        foreach (FileType type in _snapshot.Detectable)
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
    public FileType? Detect(Stream stream) => HeaderReader.Read(stream, HeaderSize, Detect);

    /// <summary>
    /// Detects the type of a file from the start of a stream asynchronously. Reads at most <see cref="HeaderSize"/> bytes and restores the position of a seekable stream.
    /// </summary>
    /// <param name="stream">The stream, positioned at the start of the file.</param>
    /// <param name="cancellationToken">A token to cancel the read.</param>
    /// <returns>The first matching type, or null.</returns>
    public ValueTask<FileType?> DetectAsync(Stream stream, CancellationToken cancellationToken = default) =>
        HeaderReader.ReadAsync(stream, HeaderSize, Detect, cancellationToken);

    /// <summary>
    /// Finds a type by the extension of a file name or by an extension, ignoring case.
    /// </summary>
    /// <param name="fileNameOrExtension">A file name or path ("photo.JPG"), an extension with a dot (".jpg") or without one ("jpg").</param>
    /// <returns>The type, or null if the extension is unknown or missing.</returns>
    public FileType? FromExtension(string? fileNameOrExtension)
    {
        string? extension = FileNames.GetExtension(fileNameOrExtension);
        return extension is not null && _snapshot.ByExtension.TryGetValue(extension, out FileType? type) ? type : null;
    }

    /// <summary>
    /// Finds a type by its MIME type, ignoring case and parameters such as "; charset=utf-8".
    /// </summary>
    /// <param name="mimeType">The MIME type, for example "image/jpeg" or "image/x-icon".</param>
    /// <returns>The type, or null if the MIME type is unknown.</returns>
    public FileType? FromMimeType(string? mimeType)
    {
        if (string.IsNullOrWhiteSpace(mimeType))
            return null;

        int separator = mimeType.IndexOf(';');
        string key = (separator >= 0 ? mimeType[..separator] : mimeType).Trim().ToLowerInvariant();
        return _snapshot.ByMimeType.TryGetValue(key, out FileType? type) ? type : null;
    }

    /// <summary>
    /// Returns the MIME type for a file name, for example for a Content-Type header.
    /// </summary>
    /// <param name="fileNameOrExtension">A file name, path or extension.</param>
    /// <returns>The MIME type, or <see cref="DefaultMimeType"/> if the extension is unknown.</returns>
    public string GetMimeType(string? fileNameOrExtension) => FromExtension(fileNameOrExtension)?.MimeType ?? DefaultMimeType;

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
    public bool ContentMatchesExtension(ReadOnlySpan<byte> header, string? fileName)
    {
        FileType? expected = FromExtension(fileName);
        FileType? detected = Detect(header);
        return expected is not null && detected is not null && detected.IsCompatibleWith(expected);
    }

    /// <summary>
    /// Checks whether the start of a stream fits the extension of a file name. Reads at most <see cref="HeaderSize"/> bytes and restores the position of a seekable stream.
    /// </summary>
    /// <param name="stream">The stream, positioned at the start of the file.</param>
    /// <param name="fileName">The file name or extension.</param>
    /// <returns>True if both are known and fit.</returns>
    public bool ContentMatchesExtension(Stream stream, string? fileName) =>
        HeaderReader.Read(stream, HeaderSize, header => ContentMatchesExtension(header, fileName));

    // An immutable view of the registry; Register swaps it as a whole.
    private sealed class Snapshot
    {
        public Snapshot(FileType[] registered, IReadOnlyList<FileType> baseTypes)
        {
            Registered = registered;
            Ordered = [.. registered, .. baseTypes];
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

        public FileType[] Registered { get; }

        public FileType[] Ordered { get; }

        public FileType[] Detectable { get; }

        public Dictionary<string, FileType> ByExtension { get; } = new(StringComparer.Ordinal);

        public Dictionary<string, FileType> ByMimeType { get; } = new(StringComparer.Ordinal);
    }
}
