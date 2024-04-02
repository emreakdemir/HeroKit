namespace HeroKit.Guids;

/// <summary>
/// Contains extension methods for <see cref="Guid"/> type.
/// </summary>
public static class GuidExtensions
{
    /// <summary>
    /// Determines whether a <see cref="Guid"/> is empty.
    /// </summary>
    /// <param name="target">The <see cref="Guid"/> to check.</param>
    /// <returns>true if the <see cref="Guid"/> is empty; otherwise, false.</returns>
    public static bool IsEmpty(this Guid target) => target == Guid.Empty;

    /// <summary>
    /// Determines whether a <see cref="Guid"/> is not empty.
    /// </summary>
    /// <param name="target">The <see cref="Guid"/> to check.</param>
    /// <returns>true if the <see cref="Guid"/> is not empty; otherwise, false.</returns>
    public static bool IsNotEmpty(this Guid target) => !target.IsEmpty();

    /// <summary>
    /// Determines whether a <see cref="Guid"/> is null or empty.
    /// </summary>
    /// <param name="target">The <see cref="Guid"/> to check.</param>
    /// <returns>true if the <see cref="Guid"/> is null or empty; otherwise, false.</returns>
    public static bool IsNullOrEmpty(this Guid? target) => !target.HasValue || target.Value.IsEmpty();

    /// <summary>
    /// Determines whether a <see cref="Guid"/> is not null or empty.
    /// </summary>
    /// <param name="target">The <see cref="Guid"/> to check.</param>
    /// <returns>true if the <see cref="Guid"/> is not null or empty; otherwise, false.</returns>
    public static bool IsNotNullOrEmpty(this Guid? target) => !target.IsNullOrEmpty();
}