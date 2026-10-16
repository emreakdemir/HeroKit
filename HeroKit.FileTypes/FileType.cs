namespace HeroKit.FileTypes;

/// <summary>
/// A file type: name, MIME type, extensions and the detectors that recognize its content. Built-in types are in <see cref="FileTypes"/>.
/// </summary>
/// <remarks>
/// Create an instance and register it with <see cref="FileTypes.Register"/> or <see cref="FileTypeRegistry.Register"/>; registered types are checked before the built-in types.
/// </remarks>
/// <example>
/// <code>
/// var sketch = new FileType("Sketch document", "application/x-sketch", FileTypeCategory.Document, "sketch")
/// {
///     Detectors = [new ZipEntryDetector("document.json")],
///     Parent = FileTypes.Zip,
/// };
/// FileTypes.Register(sketch);
/// </code>
/// </example>
public sealed class FileType
{
    /// <summary>
    /// Creates a file type.
    /// </summary>
    /// <param name="name">A readable name, for example "PNG image".</param>
    /// <param name="mimeType">The MIME type, for example "image/png".</param>
    /// <param name="category">The broad group of the type.</param>
    /// <param name="extensions">The file extensions without a dot, the usual one first, for example "jpg", "jpeg".</param>
    /// <exception cref="ArgumentException"><paramref name="name"/> or <paramref name="mimeType"/> is empty.</exception>
    public FileType(string name, string mimeType, FileTypeCategory category, params string[] extensions)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(mimeType);
        ArgumentNullException.ThrowIfNull(extensions);

        Name = name;
        MimeType = mimeType.Trim().ToLowerInvariant();
        Category = category;
        Extensions = [.. extensions.Select(e => e.Trim().TrimStart('.').ToLowerInvariant()).Where(e => e.Length > 0)];
    }

    /// <summary>Gets the readable name.</summary>
    public string Name { get; }

    /// <summary>Gets the MIME type in lower case.</summary>
    public string MimeType { get; }

    /// <summary>Gets other MIME types that are used for the same files, for example "image/x-icon".</summary>
    public IReadOnlyList<string> OtherMimeTypes { get; init; } = [];

    /// <summary>Gets the broad group.</summary>
    public FileTypeCategory Category { get; }

    /// <summary>Gets the extensions in lower case without a dot, the usual one first.</summary>
    public IReadOnlyList<string> Extensions { get; }

    /// <summary>Gets the usual extension without a dot, or an empty string.</summary>
    public string Extension => Extensions.Count > 0 ? Extensions[0] : string.Empty;

    /// <summary>
    /// Gets the detectors that recognize the content. A type without detectors is known by its extension and MIME type only.
    /// </summary>
    public IReadOnlyList<IFileTypeDetector> Detectors { get; init; } = [];

    /// <summary>
    /// Gets the more general type this type is built on, or null. For example, DOCX is a ZIP file and JSON is plain text.
    /// </summary>
    /// <remarks>
    /// <see cref="FileTypeRegistry.ContentMatchesExtension(ReadOnlySpan{byte}, string)"/> accepts content of a parent type for a more specific extension,
    /// because the content can only be recognized as far as the header shows.
    /// </remarks>
    public FileType? Parent { get; init; }

    /// <summary>
    /// Gets whether files of this type can run code: <see cref="FileTypeCategory.Executable"/> or <see cref="FileTypeCategory.Script"/>.
    /// </summary>
    public bool IsExecutable => Category is FileTypeCategory.Executable or FileTypeCategory.Script;

    /// <summary>
    /// Checks whether the first bytes of a file match one of the detectors.
    /// </summary>
    /// <param name="header">The first bytes of the file.</param>
    /// <returns>True if a detector matches.</returns>
    public bool IsMatch(ReadOnlySpan<byte> header)
    {
        foreach (IFileTypeDetector detector in Detectors)
        {
            if (detector.IsMatch(header))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether this type is <paramref name="other"/> or is built on it through <see cref="Parent"/>.
    /// </summary>
    /// <param name="other">The other type.</param>
    /// <returns>True if this type is the other type or one of its descendants.</returns>
    public bool IsA(FileType other)
    {
        ArgumentNullException.ThrowIfNull(other);
        for (FileType? type = this; type is not null; type = type.Parent)
        {
            if (ReferenceEquals(type, other))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether content of this type fits a file that claims to be <paramref name="other"/>:
    /// the types are the same, have the same MIME type, or one is built on the other through <see cref="Parent"/>.
    /// </summary>
    /// <param name="other">The type the file claims to be, for example the type of its extension.</param>
    /// <returns>True if the types fit.</returns>
    public bool IsCompatibleWith(FileType other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return IsA(other) || other.IsA(this) || string.Equals(MimeType, other.MimeType, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override string ToString() => MimeType;
}
