namespace HeroKit.FileTypes;

/// <summary>
/// Recognizes a file type from the first bytes of a file.
/// </summary>
/// <remarks>
/// Implement this interface to detect your own file types and pass the detector to <see cref="FileType.Detectors"/>.
/// Built-in detectors are <see cref="MagicNumber"/>, <see cref="ZipEntryDetector"/> and <see cref="IsoMediaDetector"/>.
/// </remarks>
public interface IFileTypeDetector
{
    /// <summary>
    /// Checks whether the first bytes of a file belong to the file type.
    /// </summary>
    /// <param name="header">The first bytes of the file; up to <see cref="FileTypeRegistry.HeaderSize"/> bytes when read from a stream. It can be shorter than the file.</param>
    /// <returns>True if the bytes match.</returns>
    bool IsMatch(ReadOnlySpan<byte> header);
}
