using HeroKit.Enums;
using HeroKit.Objects;
using HeroKit.Strings;
using HeroKit.Types;

// Exercises AOT-safe APIs and exits with a non-zero code on any unexpected result.
var checks = new (string Name, bool Passed)[]
{
    ("Enum ToList", string.Join(",", EnumExtensions.ToList<DayOfWeek>()) == "Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday"),
    ("ToEnum", "Friday".ToEnum<DayOfWeek>() == DayOfWeek.Friday),
    ("ConvertTo", ((object)"42").ConvertTo<int>() == 42),
    ("ToSlug", "Çağdaş Şehir İstanbul".ToSlug() == "cagdas-sehir-istanbul"),
    ("GetDefault", (int)typeof(int).GetDefault()! == 0),
    ("ToDecimal", "1.234,56".ToDecimal() == 1234.56m),
};

foreach (var (name, passed) in checks)
    Console.WriteLine($"{(passed ? "PASS" : "FAIL")} {name}");

return checks.All(c => c.Passed) ? 0 : 1;
