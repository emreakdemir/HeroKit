using System.Buffers.Binary;
using System.Text.Unicode;

namespace HeroKit.FileTypes;

// Detectors for built-in types that need more than fixed bytes.
internal sealed class DelegateDetector(DelegateDetector.Match match) : IFileTypeDetector
{
    public delegate bool Match(ReadOnlySpan<byte> header);

    public bool IsMatch(ReadOnlySpan<byte> header) => match(header);
}

internal static class Sniffers
{
    // "%PDF-" may follow a few junk bytes; readers accept it within the first 1024 bytes.
    public static bool Pdf(ReadOnlySpan<byte> header) =>
        header[..Math.Min(header.Length, 1024)].IndexOf("%PDF-"u8) >= 0;

    public static bool WebM(ReadOnlySpan<byte> header) =>
        header.StartsWith((ReadOnlySpan<byte>)[0x1A, 0x45, 0xDF, 0xA3]) && header[..Math.Min(header.Length, 64)].IndexOf("webm"u8) >= 0;

    // CA FE BA BE is shared by Java class files and Mach-O universal binaries.
    // Class files store the major version (45 or more) in bytes 6–7; universal binaries store a small architecture count in bytes 4–7.
    public static bool JavaClass(ReadOnlySpan<byte> header) =>
        header.Length >= 8 && header.StartsWith((ReadOnlySpan<byte>)[0xCA, 0xFE, 0xBA, 0xBE]) && BinaryPrimitives.ReadUInt16BigEndian(header[6..]) >= 45;

    public static bool MachO(ReadOnlySpan<byte> header)
    {
        if (header.Length < 8)
            return false;

        uint magic = BinaryPrimitives.ReadUInt32BigEndian(header);
        if (magic is 0xFEEDFACE or 0xFEEDFACF or 0xCEFAEDFE or 0xCFFAEDFE)
            return true;

        return magic == 0xCAFEBABE && BinaryPrimitives.ReadUInt32BigEndian(header[4..]) is >= 1 and <= 30;
    }

    public static bool Shebang(ReadOnlySpan<byte> header) =>
        header.Length >= 3 && header[0] == '#' && header[1] == '!' && (header[2] == '/' || header[2] == ' ');

    public static bool PlainText(ReadOnlySpan<byte> header)
    {
        if (header.StartsWith((ReadOnlySpan<byte>)[0xFF, 0xFE]) || header.StartsWith((ReadOnlySpan<byte>)[0xFE, 0xFF]))
            return true;

        return TryGetUtf8Text(header, out _);
    }

    public static bool Json(ReadOnlySpan<byte> header)
    {
        if (!TryGetUtf8Text(header, out ReadOnlySpan<byte> text))
            return false;

        text = TrimStart(text);
        if (text.Length < 2)
            return false;

        ReadOnlySpan<byte> next = TrimStart(text[1..]);
        if (next.IsEmpty)
            return false;

        return text[0] switch
        {
            (byte)'{' => next[0] is (byte)'"' or (byte)'}',
            (byte)'[' => next[0] is (byte)'{' or (byte)'[' or (byte)'"' or (byte)']' or (byte)'-' or (>= (byte)'0' and <= (byte)'9') or (byte)'t' or (byte)'f' or (byte)'n',
            _ => false,
        };
    }

    public static bool Xml(ReadOnlySpan<byte> header) =>
        TryGetUtf8Text(header, out ReadOnlySpan<byte> text) && TrimStart(text).StartsWith("<?xml"u8);

    public static bool Svg(ReadOnlySpan<byte> header)
    {
        if (!TryGetUtf8Text(header, out ReadOnlySpan<byte> text))
            return false;

        text = TrimStart(text);
        return text.StartsWith("<"u8) && IndexOfIgnoreCase(text, "<svg"u8) >= 0;
    }

    public static bool Html(ReadOnlySpan<byte> header)
    {
        if (!TryGetUtf8Text(header, out ReadOnlySpan<byte> text))
            return false;

        text = TrimStart(text);
        return StartsWithIgnoreCase(text, "<!doctype html"u8) || StartsWithIgnoreCase(text, "<html"u8)
            || StartsWithIgnoreCase(text, "<head"u8) || StartsWithIgnoreCase(text, "<body"u8) || StartsWithIgnoreCase(text, "<script"u8);
    }

    // UTF-8 text without control characters other than tab, line feed, form feed, carriage return and escape.
    // A multi-byte character cut off at the end of the header is allowed.
    public static bool TryGetUtf8Text(ReadOnlySpan<byte> header, out ReadOnlySpan<byte> text)
    {
        text = header.StartsWith((ReadOnlySpan<byte>)[0xEF, 0xBB, 0xBF]) ? header[3..] : header;
        if (text.IsEmpty)
            return false;

        foreach (byte b in text)
        {
            if (b < 0x20 && b is not (0x09 or 0x0A or 0x0C or 0x0D or 0x1B))
                return false;
        }

        for (int cut = 0; cut <= 3 && cut < text.Length; cut++)
        {
            if (Utf8.IsValid(text[..^cut]))
                return cut == 0 || text[^cut] >= 0xC0;
        }

        return false;
    }

    public static int IndexOfIgnoreCase(ReadOnlySpan<byte> text, ReadOnlySpan<byte> lowerValue)
    {
        for (int i = 0; i + lowerValue.Length <= text.Length; i++)
        {
            if (StartsWithIgnoreCase(text[i..], lowerValue))
                return i;
        }

        return -1;
    }

    public static bool StartsWithIgnoreCase(ReadOnlySpan<byte> text, ReadOnlySpan<byte> lowerValue)
    {
        if (text.Length < lowerValue.Length)
            return false;

        for (int i = 0; i < lowerValue.Length; i++)
        {
            byte c = text[i];
            if (c is >= (byte)'A' and <= (byte)'Z')
                c = (byte)(c + 32);

            if (c != lowerValue[i])
                return false;
        }

        return true;
    }

    private static ReadOnlySpan<byte> TrimStart(ReadOnlySpan<byte> text)
    {
        int i = 0;
        while (i < text.Length && text[i] is (byte)' ' or (byte)'\t' or (byte)'\r' or (byte)'\n')
            i++;

        return text[i..];
    }
}
