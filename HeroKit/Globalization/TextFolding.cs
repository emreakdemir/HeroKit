namespace HeroKit.Globalization;

// Lower-cases with the given rules and removes diacritics, for comparisons that ignore case and accents.
internal static class TextFolding
{
    public static string Fold(string value, ITextRules? rules)
    {
        return string.Create(value.Length, (value, rules), static (destination, state) =>
        {
            for (int i = 0; i < state.value.Length; i++)
            {
                char lower = state.rules?.ToLower(state.value[i]) ?? char.ToLowerInvariant(state.value[i]);
                destination[i] = RemoveDiacritic(lower);
            }
        });
    }

    public static char RemoveDiacritic(char c) => Strings.Diacritics.Remove(c);
}
