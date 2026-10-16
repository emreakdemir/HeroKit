using System.Buffers.Binary;
using System.Text;

namespace HeroKit.FileTypes;

/// <summary>
/// Detects ISO base media files (MP4, MOV, M4A, 3GP, HEIC, AVIF) by the brands in the "ftyp" box.
/// </summary>
/// <example>
/// <code>
/// new IsoMediaDetector("M4A ", "M4B ");                                // major brand only
/// new IsoMediaDetector("avif", "avis") { IncludeCompatibleBrands = true }; // also compatible brands
/// </code>
/// </example>
public sealed class IsoMediaDetector : IFileTypeDetector
{
    private readonly uint[] _brands;

    /// <summary>
    /// Creates a detector for the given four-character brands, for example "isom" or "qt  ".
    /// </summary>
    /// <param name="brands">The brands, each exactly four ASCII characters.</param>
    /// <exception cref="ArgumentException">A brand is not four characters long, or no brand is given.</exception>
    public IsoMediaDetector(params string[] brands)
    {
        ArgumentNullException.ThrowIfNull(brands);
        if (brands.Length == 0)
            throw new ArgumentException("At least one brand is required.", nameof(brands));

        _brands = new uint[brands.Length];
        for (int i = 0; i < brands.Length; i++)
        {
            if (brands[i] is not { Length: 4 })
                throw new ArgumentException($"The brand '{brands[i]}' must have four characters.", nameof(brands));

            _brands[i] = BinaryPrimitives.ReadUInt32BigEndian(Encoding.ASCII.GetBytes(brands[i]));
        }
    }

    /// <summary>
    /// Gets whether the compatible brands also count. When false, only the major brand is checked.
    /// </summary>
    public bool IncludeCompatibleBrands { get; init; }

    /// <inheritdoc />
    public bool IsMatch(ReadOnlySpan<byte> header)
    {
        if (header.Length < 12 || !header.Slice(4, 4).SequenceEqual("ftyp"u8))
            return false;

        if (Contains(BinaryPrimitives.ReadUInt32BigEndian(header[8..])))
            return true;

        if (!IncludeCompatibleBrands)
            return false;

        long boxSize = BinaryPrimitives.ReadUInt32BigEndian(header);
        int end = (int)Math.Min(boxSize, header.Length);
        for (int position = 16; position + 4 <= end; position += 4)
        {
            if (Contains(BinaryPrimitives.ReadUInt32BigEndian(header[position..])))
                return true;
        }

        return false;
    }

    private bool Contains(uint brand) => Array.IndexOf(_brands, brand) >= 0;
}
