using System.Collections.Frozen;

namespace HeroKit.Strings;

// One-to-one map from accented Latin letters to their base letter, without ICU or Unicode normalization,
// so it behaves the same with invariant globalization. Covers Turkish, Latin-1 and common Central European letters.
internal static class Diacritics
{
    private const string From = "ÀÁÂÃÄÅàáâãäåÇçĆćČčĎďĐđÈÉÊËèéêëĘęĚěĞğÌÍÎÏìíîïİıŁłĹĺĽľÑñŃńŇňÒÓÔÕÖØòóôõöøŐőŔŕŘřŚśŞşŠšŤťÙÚÛÜùúûüŮůŰűÝýÿŹźŻżŽž";
    private const string To   = "AAAAAAaaaaaaCcCcCcDdDdEEEEeeeeEeEeGgIIIIiiiiIiLlLlLlNnNnNnOOOOOOooooooOoRrRrSsSsSsTtUUUUuuuuUuUuYyyZzZzZz";

    private static readonly FrozenDictionary<char, char> Map = BuildMap();

    public static char Remove(char c) => c < 128 ? c : Map.GetValueOrDefault(c, c);

    public static string Remove(string value)
    {
        int first = 0;
        while (first < value.Length && Remove(value[first]) == value[first])
            first++;

        if (first == value.Length)
            return value;

        return string.Create(value.Length, value, static (destination, source) =>
        {
            for (int i = 0; i < source.Length; i++)
                destination[i] = Remove(source[i]);
        });
    }

    private static FrozenDictionary<char, char> BuildMap()
    {
        if (From.Length != To.Length)
            throw new InvalidOperationException("Diacritics map is out of sync.");

        var map = new Dictionary<char, char>(From.Length);
        for (int i = 0; i < From.Length; i++)
            map[From[i]] = To[i];

        return map.ToFrozenDictionary();
    }
}
