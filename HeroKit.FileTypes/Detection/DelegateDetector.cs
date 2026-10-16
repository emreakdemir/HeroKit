namespace HeroKit.FileTypes;

// Adapts a sniffer method to IFileTypeDetector for built-in types that need more than fixed bytes.
internal sealed class DelegateDetector(DelegateDetector.Match match) : IFileTypeDetector
{
    public delegate bool Match(ReadOnlySpan<byte> header);

    public bool IsMatch(ReadOnlySpan<byte> header) => match(header);
}
