using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace HeroKit.Enums;

/// <summary>
/// Reads display texts of enum values.
/// </summary>
public static class EnumDescriptionExtensions
{
    /// <summary>
    /// Returns the display text of an enum value from <see cref="DescriptionAttribute"/> or <see cref="DisplayAttribute"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="DescriptionAttribute"/> wins over <see cref="DisplayAttribute.Name"/>. Values without either attribute,
    /// and combined flag values, return their name.
    /// </remarks>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The enum value.</param>
    /// <returns>The description, the display name or the member name.</returns>
    /// <example>
    /// <code>
    /// enum OrderStatus { [Description("Kargoya verildi")] Shipped }
    /// OrderStatus.Shipped.GetDescription(); // "Kargoya verildi"
    /// </code>
    /// </example>
    public static string GetDescription<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>(this T value)
        where T : struct, Enum
    {
        string name = value.ToString();
        FieldInfo? field = typeof(T).GetField(name, BindingFlags.Public | BindingFlags.Static);

        if (field is null)
            return name;

        if (field.GetCustomAttribute<DescriptionAttribute>(inherit: false) is { Description.Length: > 0 } description)
            return description.Description;

        return field.GetCustomAttribute<DisplayAttribute>(inherit: false)?.GetName() ?? name;
    }
}
