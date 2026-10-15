namespace HeroKit.Decimals;

/// <summary>
/// Provides price rounding, percentage and VAT helpers for decimal values.
/// Methods return unrounded values unless a <c>decimals</c> argument is given.
/// Callers round money with <c>Math.Round(x, 2, MidpointRounding.AwayFromZero)</c>.
/// </summary>
public static class DecimalPriceExtensions
{
    /// <summary>
    /// Rounds a value to the nearest multiple of the step.
    /// </summary>
    /// <param name="value">The value to round.</param>
    /// <param name="step">The step size. Must be greater than zero.</param>
    /// <param name="mode">The midpoint rounding mode. The default is AwayFromZero.</param>
    /// <returns>The nearest multiple of <paramref name="step"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="step"/> is zero or negative.</exception>
    /// <example><code>
    /// 12.37m.RoundToStep(0.05m); // 12.35
    /// 2.675m.RoundToStep(0.01m); // 2.68
    /// </code></example>
    public static decimal RoundToStep(this decimal value, decimal step, MidpointRounding mode = MidpointRounding.AwayFromZero)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(step, 0m);
        return Math.Round(value / step, 0, mode) * step;
    }

    /// <summary>
    /// Raises a price to the next price that ends with the given fraction (for example 12.99).
    /// The result is floor(value) + ending, plus 1 if that is below the value.
    /// </summary>
    /// <param name="value">The price. Must not be negative.</param>
    /// <param name="ending">The fractional ending, from 0 up to but not including 1. The default is 0.99.</param>
    /// <returns>The psychological price that is greater than or equal to <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative or <paramref name="ending"/> is outside [0, 1).</exception>
    /// <example><code>
    /// 12.30m.ToPsychologicalPrice(); // 12.99
    /// 12.60m.ToPsychologicalPrice(0.50m); // 13.50
    /// </code></example>
    public static decimal ToPsychologicalPrice(this decimal value, decimal ending = 0.99m)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        if (ending < 0m || ending >= 1m)
            throw new ArgumentOutOfRangeException(nameof(ending), ending, "Ending must be in the range [0, 1).");

        decimal result = Math.Floor(value) + ending;
        if (result < value)
            result += 1m;

        return result;
    }

    /// <summary>
    /// Adds a percentage to a value: value * (1 + percent / 100). The result is not rounded.
    /// </summary>
    /// <param name="value">The base value.</param>
    /// <param name="percent">The percentage to add.</param>
    /// <returns>The increased value.</returns>
    /// <example><code>
    /// 200m.AddPercent(10m); // 220
    /// </code></example>
    public static decimal AddPercent(this decimal value, decimal percent) => value * (1m + percent / 100m);

    /// <summary>
    /// Subtracts a percentage from a value: value * (1 - percent / 100). The result is not rounded.
    /// </summary>
    /// <param name="value">The base value.</param>
    /// <param name="percent">The percentage to subtract.</param>
    /// <returns>The decreased value.</returns>
    /// <example><code>
    /// 200m.SubtractPercent(25m); // 150
    /// </code></example>
    public static decimal SubtractPercent(this decimal value, decimal percent) => value * (1m - percent / 100m);

    /// <summary>
    /// Adds VAT to a net amount: net * (1 + rate / 100).
    /// </summary>
    /// <param name="netAmount">The net amount.</param>
    /// <param name="ratePercent">The VAT rate in percent. Must not be negative.</param>
    /// <param name="decimals">When given, rounds the result to this many decimals with AwayFromZero. Otherwise the result is not rounded.</param>
    /// <returns>The gross amount.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="ratePercent"/> is negative.</exception>
    /// <example><code>
    /// 100m.IncludeVat(20m); // 120
    /// </code></example>
    public static decimal IncludeVat(this decimal netAmount, decimal ratePercent, int? decimals = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(ratePercent);
        return Round(netAmount * (1m + ratePercent / 100m), decimals);
    }

    /// <summary>
    /// Removes VAT from a gross amount: gross / (1 + rate / 100).
    /// </summary>
    /// <param name="grossAmount">The gross amount.</param>
    /// <param name="ratePercent">The VAT rate in percent. Must not be negative.</param>
    /// <param name="decimals">When given, rounds the result to this many decimals with AwayFromZero. Otherwise the result is not rounded.</param>
    /// <returns>The net amount.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="ratePercent"/> is negative.</exception>
    /// <example><code>
    /// 120m.ExcludeVat(20m); // 100
    /// </code></example>
    public static decimal ExcludeVat(this decimal grossAmount, decimal ratePercent, int? decimals = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(ratePercent);
        return Round(grossAmount / (1m + ratePercent / 100m), decimals);
    }

    /// <summary>
    /// Returns the VAT part of a gross amount: gross - ExcludeVat(gross, rate).
    /// </summary>
    /// <param name="grossAmount">The gross amount.</param>
    /// <param name="ratePercent">The VAT rate in percent. Must not be negative.</param>
    /// <param name="decimals">When given, rounds the result to this many decimals with AwayFromZero. Otherwise the result is not rounded.</param>
    /// <returns>The VAT amount.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="ratePercent"/> is negative.</exception>
    /// <example><code>
    /// 120m.VatAmountFromGross(20m); // 20
    /// </code></example>
    public static decimal VatAmountFromGross(this decimal grossAmount, decimal ratePercent, int? decimals = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(ratePercent);
        return Round(grossAmount - grossAmount / (1m + ratePercent / 100m), decimals);
    }

    private static decimal Round(decimal value, int? decimals) =>
        decimals is null ? value : Math.Round(value, decimals.Value, MidpointRounding.AwayFromZero);
}
