using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using HeroKit.Strings;

namespace HeroKit.Globalization.Turkey;

/// <summary>
/// A Turkish identity number (T.C. Kimlik Numarası) that has passed the checksum check. Validate once at the edge, then pass the value around.
/// </summary>
/// <remarks>
/// <para>Create values with <see cref="Parse(string)"/> or <see cref="TryParse(string?, out Tckn)"/>. The type implements <see cref="IParsable{TSelf}"/>,
/// so ASP.NET Core minimal APIs bind it from route and query values, and it has a JSON converter and a type converter for MVC model binding
/// and configuration.</para>
/// <para><c>default(Tckn)</c> holds no number: <see cref="IsEmpty"/> is true and <see cref="Value"/> is an empty string.</para>
/// </remarks>
/// <example>
/// <code>
/// Tckn id = Tckn.Parse("10000000146");
/// id.Value;             // "10000000146"
/// id.ToMaskedString();  // "10*******46"
///
/// app.MapGet("/people/{id}", (Tckn id) => ...); // invalid numbers return 400
/// </code>
/// </example>
[JsonConverter(typeof(TcknJsonConverter))]
[TypeConverter(typeof(TcknTypeConverter))]
public readonly struct Tckn : IEquatable<Tckn>, IComparable<Tckn>, IParsable<Tckn>
{
    private readonly string? _value;

    private Tckn(string value) => _value = value;

    /// <summary>Gets the 11 digits, or an empty string for <c>default(Tckn)</c>.</summary>
    public string Value => _value ?? string.Empty;

    /// <summary>Gets whether this is <c>default(Tckn)</c>, which holds no number.</summary>
    public bool IsEmpty => _value is null;

    /// <summary>
    /// Converts a string to a <see cref="Tckn"/>.
    /// </summary>
    /// <param name="value">11 digits with a valid checksum.</param>
    /// <returns>The identity number.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
    /// <exception cref="FormatException"><paramref name="value"/> is not a valid TCKN.</exception>
    public static Tckn Parse(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return TryParse(value, out Tckn result) ? result : throw new FormatException("The value is not a valid Turkish identity number (TCKN).");
    }

    /// <summary>
    /// Tries to convert a string to a <see cref="Tckn"/>.
    /// </summary>
    /// <param name="value">11 digits with a valid checksum.</param>
    /// <param name="result">The identity number when the method returns true; otherwise <c>default</c>.</param>
    /// <returns>True if the value is a valid TCKN.</returns>
    public static bool TryParse([NotNullWhen(true)] string? value, out Tckn result)
    {
        result = value.IsValidTckn() ? new Tckn(value!) : default;
        return !result.IsEmpty;
    }

    static Tckn IParsable<Tckn>.Parse(string s, IFormatProvider? provider) => Parse(s);

    static bool IParsable<Tckn>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Tckn result) => TryParse(s, out result);

    /// <summary>
    /// Returns the number with the middle digits hidden, for logs and screens: "10*******46".
    /// </summary>
    /// <returns>The masked number, or an empty string for <c>default(Tckn)</c>.</returns>
    public string ToMaskedString() => Value.MaskMiddle(2, 2);

    /// <summary>Returns the 11 digits.</summary>
    /// <returns><see cref="Value"/>.</returns>
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(Tckn other) => string.Equals(_value, other._value, StringComparison.Ordinal);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Tckn other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => _value is null ? 0 : StringComparer.Ordinal.GetHashCode(_value);

    /// <inheritdoc />
    public int CompareTo(Tckn other) => string.CompareOrdinal(_value, other._value);

    /// <summary>Compares two identity numbers for equality.</summary>
    /// <param name="left">The first number.</param>
    /// <param name="right">The second number.</param>
    /// <returns>True if both hold the same number.</returns>
    public static bool operator ==(Tckn left, Tckn right) => left.Equals(right);

    /// <summary>Compares two identity numbers for inequality.</summary>
    /// <param name="left">The first number.</param>
    /// <param name="right">The second number.</param>
    /// <returns>True if the numbers differ.</returns>
    public static bool operator !=(Tckn left, Tckn right) => !left.Equals(right);

    /// <summary>Returns the 11 digits.</summary>
    /// <param name="value">The identity number.</param>
    public static implicit operator string(Tckn value) => value.Value;

    /// <summary>Converts a string with <see cref="Parse(string)"/>.</summary>
    /// <param name="value">11 digits with a valid checksum.</param>
    /// <exception cref="FormatException"><paramref name="value"/> is not a valid TCKN.</exception>
    public static explicit operator Tckn(string value) => Parse(value);
}

/// <summary>
/// Reads and writes a <see cref="Tckn"/> as a JSON string. An invalid number fails deserialization; <c>default(Tckn)</c> is written as null.
/// </summary>
/// <remarks>
/// <see cref="Tckn"/> uses this converter through <see cref="JsonConverterAttribute"/>. It is public so that the System.Text.Json source generator,
/// which Native AOT applications use, can create it.
/// </remarks>
public sealed class TcknJsonConverter : JsonConverter<Tckn>
{
    /// <inheritdoc />
    public override Tckn Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.String && Tckn.TryParse(reader.GetString(), out Tckn value)
            ? value
            : throw new JsonException("The value is not a valid Turkish identity number (TCKN).");

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Tckn value, JsonSerializerOptions options)
    {
        if (value.IsEmpty)
            writer.WriteNullValue();
        else
            writer.WriteStringValue(value.Value);
    }
}

// Converts between Tckn and string for MVC model binding and configuration binding.
internal sealed class TcknTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) =>
        destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) =>
        value is string text ? Tckn.Parse(text.Trim()) : base.ConvertFrom(context, culture, value);

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) =>
        destinationType == typeof(string) && value is Tckn tckn ? tckn.Value : base.ConvertTo(context, culture, value, destinationType);
}
