namespace HeroKit.Arrays;

/// <summary>
/// Provides extension methods for converting arrays.
/// </summary>
public static class ArrayConversionExtensions
{
    /// <summary>
    /// Encodes bytes as URL-safe base64 (RFC 4648 §5): '-' and '_' instead of '+' and '/', without padding.
    /// </summary>
    /// <param name="value">The bytes to encode.</param>
    /// <returns>The encoded string, or an empty string if <paramref name="value"/> is null or empty.</returns>
    /// <example>
    /// <code>
    /// new byte[] { 251, 255 }.ToBase64Url(); // "-_8"
    /// </code>
    /// </example>
    public static string ToBase64Url(this byte[]? value)
    {
        if (value is not { Length: > 0 })
            return string.Empty;

        return Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }
}
