using System.Buffers.Binary;

namespace HeroKit.FileTypes;

// Binary formats whose signature is not a fixed byte sequence at a fixed offset.
internal static class BinarySniffers
{
    private static ReadOnlySpan<byte> EbmlHeader => [0x1A, 0x45, 0xDF, 0xA3];

    private static ReadOnlySpan<byte> FatOrClassMagic => [0xCA, 0xFE, 0xBA, 0xBE];

    // "%PDF-" may follow a few junk bytes; readers accept it within the first 1024 bytes.
    public static bool Pdf(ReadOnlySpan<byte> header) =>
        header[..Math.Min(header.Length, 1024)].IndexOf("%PDF-"u8) >= 0;

    // WebM is Matroska with the DocType "webm" in the EBML header.
    public static bool WebM(ReadOnlySpan<byte> header) =>
        header.StartsWith(EbmlHeader) && header[..Math.Min(header.Length, 64)].IndexOf("webm"u8) >= 0;

    // CA FE BA BE is shared by Java class files and Mach-O universal binaries.
    // Class files store the major version (45 or more) in bytes 6–7; universal binaries store a small architecture count in bytes 4–7.
    public static bool JavaClass(ReadOnlySpan<byte> header) =>
        header.Length >= 8 && header.StartsWith(FatOrClassMagic) && BinaryPrimitives.ReadUInt16BigEndian(header[6..]) >= 45;

    public static bool MachO(ReadOnlySpan<byte> header)
    {
        if (header.Length < 8)
            return false;

        uint magic = BinaryPrimitives.ReadUInt32BigEndian(header);
        if (magic is 0xFEEDFACE or 0xFEEDFACF or 0xCEFAEDFE or 0xCFFAEDFE)
            return true;

        return magic == 0xCAFEBABE && BinaryPrimitives.ReadUInt32BigEndian(header[4..]) is >= 1 and <= 30;
    }
}
