using System.Buffers.Binary;
using System.Text;

namespace HeroKit.FileTypes;

/// <summary>
/// Detects a ZIP-based file type, such as DOCX, ODT, EPUB, JAR or APK, by the names of the entries in the header.
/// </summary>
/// <remarks>
/// The detector walks the local file headers that fit in the header bytes; it does not decompress anything.
/// Entries after the header are not seen, so put the most specific types first and give them the ZIP type as <see cref="FileType.Parent"/>.
/// </remarks>
/// <example>
/// <code>
/// new ZipEntryDetector("word/");                                       // DOCX
/// ZipEntryDetector.ForMimeTypeEntry("application/vnd.oasis.opendocument.text"); // ODT
/// </code>
/// </example>
public sealed class ZipEntryDetector : IFileTypeDetector
{
    private const uint LocalFileHeader = 0x04034B50;
    private const int LocalHeaderLength = 30;

    private readonly byte[] _entryName;
    private readonly byte[]? _mimeType;

    /// <summary>
    /// Creates a detector that matches a ZIP file with an entry whose name starts with <paramref name="entryNamePrefix"/>.
    /// </summary>
    /// <param name="entryNamePrefix">The start of the entry name, for example "word/" or "AndroidManifest.xml". Case-sensitive.</param>
    /// <exception cref="ArgumentException"><paramref name="entryNamePrefix"/> is empty.</exception>
    public ZipEntryDetector(string entryNamePrefix)
    {
        ArgumentException.ThrowIfNullOrEmpty(entryNamePrefix);
        _entryName = Encoding.UTF8.GetBytes(entryNamePrefix);
    }

    private ZipEntryDetector(byte[] mimeType)
    {
        _entryName = "mimetype"u8.ToArray();
        _mimeType = mimeType;
    }

    /// <summary>
    /// Creates a detector for OpenDocument and EPUB style files: an uncompressed "mimetype" entry that holds <paramref name="mimeType"/>.
    /// </summary>
    /// <param name="mimeType">The content of the "mimetype" entry, for example "application/epub+zip".</param>
    /// <returns>The detector.</returns>
    /// <exception cref="ArgumentException"><paramref name="mimeType"/> is empty.</exception>
    public static ZipEntryDetector ForMimeTypeEntry(string mimeType)
    {
        ArgumentException.ThrowIfNullOrEmpty(mimeType);
        return new ZipEntryDetector(Encoding.ASCII.GetBytes(mimeType));
    }

    /// <inheritdoc />
    public bool IsMatch(ReadOnlySpan<byte> header)
    {
        long position = 0;
        while (position + LocalHeaderLength <= header.Length)
        {
            ReadOnlySpan<byte> entry = header[(int)position..];
            if (BinaryPrimitives.ReadUInt32LittleEndian(entry) != LocalFileHeader)
                return false;

            ushort flags = BinaryPrimitives.ReadUInt16LittleEndian(entry[6..]);
            ushort method = BinaryPrimitives.ReadUInt16LittleEndian(entry[8..]);
            uint compressedSize = BinaryPrimitives.ReadUInt32LittleEndian(entry[18..]);
            int nameLength = BinaryPrimitives.ReadUInt16LittleEndian(entry[26..]);
            int extraLength = BinaryPrimitives.ReadUInt16LittleEndian(entry[28..]);

            if (LocalHeaderLength + nameLength > entry.Length)
                return false;

            ReadOnlySpan<byte> name = entry.Slice(LocalHeaderLength, nameLength);
            long dataStart = LocalHeaderLength + nameLength + extraLength;

            if (_mimeType is null)
            {
                if (name.StartsWith(_entryName))
                    return true;
            }
            else if (name.SequenceEqual(_entryName) && method == 0 && dataStart + _mimeType.Length <= entry.Length)
            {
                return entry.Slice((int)dataStart, _mimeType.Length).SequenceEqual(_mimeType);
            }

            // With a data descriptor the size can be unknown here, so the next entry cannot be found.
            if ((flags & 0x08) != 0 && compressedSize == 0)
                return false;

            position += dataStart + compressedSize;
        }

        return false;
    }
}
