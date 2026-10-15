using BenchmarkDotNet.Attributes;
using HeroKit.Objects;
using HeroKit.Strings;

namespace HeroKit.Benchmarks;

[MemoryDiagnoser]
public class StringBenchmarks
{
    private const string Title = "  Çağdaş Şehir Planlaması: İstanbul'da Ölçülebilir Gelişim ve Ürünler  ";
    private const string PascalCase = "HeroKitStringManipulationExtensions";

    [Benchmark]
    public string ToSlug() => Title.ToSlug();

    [Benchmark]
    public string ClearTurkishChars() => Title.ClearTurkishChars();

    [Benchmark]
    public string RemoveAccent() => Title.RemoveAccent();

    [Benchmark]
    public string ToDashCase() => PascalCase.ToDashCase();

    [Benchmark]
    public decimal ToDecimal() => "1.234,56".ToDecimal();

    [Benchmark]
    public int ConvertToInt() => ((object)"42").ConvertTo<int>();
}
