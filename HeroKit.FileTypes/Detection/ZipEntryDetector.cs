using System.Buffers.Binary;
using System.Text;

namespace HeroKit.FileTypes;

/// <summary>
/// Detects a ZIP-based file type, such as DOCX, ODT, EPUB, JAR or APK, by the names of the entries in the header.
/// </summary>
/// <remarks>
/// The detector reads entry names without decompressing anything: the local file headers from the start of the file and,
/// when <see cref="FileTypeRegistry"/> reads a seekable stream, the central directory at the end of the file. From a span with only
/// the first bytes, later entries are not seen, so give ZIP-based types the ZIP type as <see cref="FileType.Parent"/>.
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
    private const int CentralHeaderLength = 46;

    private static ReadOnlySpan<byte> CentralDirectorySignature => [0x50, 0x4B, 0x01, 0x02];

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
        if (header.Length < 4 || BinaryPrimitives.ReadUInt32LittleEndian(header) != LocalFileHeader)
            return false;

        return MatchesLocalEntries(header) || (_mimeType is null && MatchesCentralDirectory(header));
    }

    // Walks the local file headers from the start of the file.
    private bool MatchesLocalEntries(ReadOnlySpan<byte> header)
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

    // Looks for central directory records, which FileTypeRegistry appends after the header for seekable ZIP streams.
    private bool MatchesCentralDirectory(ReadOnlySpan<byte> header)
    {
        int position = 0;
        while (position < header.Length)
        {
            int found = header[position..].IndexOf(CentralDirectorySignature);
            if (found < 0)
                return false;

            int record = position + found;
            if (record + CentralHeaderLength <= header.Length)
            {
                int nameLength = BinaryPrimitives.ReadUInt16LittleEndian(header[(record + 28)..]);
                if (record + CentralHeaderLength + nameLength <= header.Length
                    && header.Slice(record + CentralHeaderLength, nameLength).StartsWith(_entryName))
                {
                    return true;
                }
            }

            position = record + 4;
        }

        return false;
    }
}
