using System.Buffers.Binary;

namespace HeroKit.FileTypes;

// Finds the central directory of a ZIP file from the end of the file (the "end of central directory" record).
// ZIP64 archives and archives with a comment longer than the searched tail are not supported; detection then uses the header only.
internal static class ZipDirectoryLocator
{
    // The end record is 22 bytes plus a comment of up to 65,535 bytes.
    public const int MaxTailLength = 22 + ushort.MaxValue;

    // Reads at most this much of the central directory, enough for about 10,000 entries.
    public const int MaxDirectoryLength = 1024 * 1024;

    private const uint EndOfCentralDirectory = 0x06054B50;

    public static bool StartsLikeZip(ReadOnlySpan<byte> header) =>
        header.Length >= 4 && BinaryPrimitives.ReadUInt32LittleEndian(header) == 0x04034B50;

    // Locates the directory from the tail of a file of fileLength bytes; offsets are from the start of the file.
    public static bool TryLocate(ReadOnlySpan<byte> tail, long fileLength, out long offset, out int length)
    {
        offset = 0;
        length = 0;
        for (int i = tail.Length - 22; i >= 0; i--)
        {
            if (BinaryPrimitives.ReadUInt32LittleEndian(tail[i..]) != EndOfCentralDirectory)
                continue;

            uint size = BinaryPrimitives.ReadUInt32LittleEndian(tail[(i + 12)..]);
            uint start = BinaryPrimitives.ReadUInt32LittleEndian(tail[(i + 16)..]);
            if (size == uint.MaxValue || start == uint.MaxValue || (long)start + size > fileLength)
                return false;

            offset = start;
            length = (int)Math.Min(size, MaxDirectoryLength);
            return length > 0;
        }

        return false;
    }
}
