using System.Globalization;
using System.Text;

namespace HeroKit.FileTypes;

/// <summary>
/// Detects a file type by fixed bytes at a fixed offset, also called a file signature.
/// </summary>
/// <example>
/// <code>
/// new MagicNumber("89 50 4E 47 0D 0A 1A 0A");          // PNG
/// new MagicNumber("52 49 46 46 ?? ?? ?? ?? 57 41 56 45"); // RIFF....WAVE, "??" matches any byte
/// MagicNumber.FromText("ustar", offset: 257);           // tar
/// </code>
/// </example>
public sealed class MagicNumber : IFileTypeDetector
{
    private readonly byte[] _bytes;
    private readonly bool[] _wildcards;
    private readonly int _offset;

    /// <summary>
    /// Creates a detector from hexadecimal bytes separated by spaces. "??" matches any byte.
    /// </summary>
    /// <param name="pattern">The bytes, for example "25 50 44 46 2D".</param>
    /// <param name="offset">The position of the first byte in the file.</param>
    /// <exception cref="ArgumentException"><paramref name="pattern"/> is empty or not hexadecimal.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
    public MagicNumber(string pattern, int offset = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(pattern);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);

        string[] parts = pattern.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        _bytes = new byte[parts.Length];
        _wildcards = new bool[parts.Length];
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] == "??")
                _wildcards[i] = true;
            else if (parts[i].Length != 2 || !byte.TryParse(parts[i], NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out _bytes[i]))
                throw new ArgumentException($"'{parts[i]}' is not a hexadecimal byte or '??'.", nameof(pattern));
        }

        _offset = offset;
    }

    private MagicNumber(byte[] bytes, int offset)
    {
        _bytes = bytes;
        _wildcards = new bool[bytes.Length];
        _offset = offset;
    }

    /// <summary>
    /// Creates a detector from ASCII text, for example "%PDF-".
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="offset">The position of the first byte in the file.</param>
    /// <returns>The detector.</returns>
    /// <exception cref="ArgumentException"><paramref name="text"/> is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
    public static MagicNumber FromText(string text, int offset = 0)
    {
        ArgumentException.ThrowIfNullOrEmpty(text);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        return new MagicNumber(Encoding.ASCII.GetBytes(text), offset);
    }

    /// <summary>Gets the number of bytes the header must have, from the start of the file.</summary>
    public int Length => _offset + _bytes.Length;

    /// <inheritdoc />
    public bool IsMatch(ReadOnlySpan<byte> header)
    {
        if (header.Length < Length)
            return false;

        ReadOnlySpan<byte> slice = header.Slice(_offset, _bytes.Length);
        for (int i = 0; i < _bytes.Length; i++)
        {
            if (!_wildcards[i] && slice[i] != _bytes[i])
                return false;
        }

        return true;
    }
}
