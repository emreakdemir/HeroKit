using System.Text;

namespace HeroKit.FileTypes;

// Finds content in an SVG that a browser can run or load: script, event handlers, embedded HTML,
// entity declarations (XXE) and external references. A DOCTYPE alone is allowed because editors write one.
internal static class SvgInspector
{
    private static readonly byte[][] Markers =
    [
        .. new[]
        {
            "<script", "javascript:", "vbscript:", "data:text/html", "<foreignobject", "<iframe", "<embed", "<object",
            "<!entity", "href=\"http", "href='http", "href=\"//", "href='//", "@import",
        }.Select(Encoding.ASCII.GetBytes),
    ];

    public static bool HasActiveContent(ReadOnlySpan<byte> svg)
    {
        foreach (byte[] marker in Markers)
        {
            if (TextSniffer.IndexOfIgnoreCase(svg, marker) >= 0)
                return true;
        }

        return HasEventHandler(svg);
    }

    // An attribute such as onload= or onclick = after whitespace or a slash.
    private static bool HasEventHandler(ReadOnlySpan<byte> svg)
    {
        for (int i = 1; i + 3 < svg.Length; i++)
        {
            if (svg[i - 1] is not ((byte)' ' or (byte)'\t' or (byte)'\r' or (byte)'\n' or (byte)'/') || !TextSniffer.StartsWithIgnoreCase(svg[i..], "on"u8))
                continue;

            int j = i + 2;
            int letters = 0;
            while (j < svg.Length && (svg[j] | 0x20) is >= (byte)'a' and <= (byte)'z')
            {
                j++;
                letters++;
            }

            while (j < svg.Length && svg[j] is (byte)' ' or (byte)'\t' or (byte)'\r' or (byte)'\n')
                j++;

            if (letters > 0 && j < svg.Length && svg[j] == '=')
                return true;
        }

        return false;
    }
}
