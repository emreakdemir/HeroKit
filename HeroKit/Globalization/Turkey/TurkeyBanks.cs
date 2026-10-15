using HeroKit.Banking;

namespace HeroKit.Globalization.Turkey;

// Banks by the 5-digit bank code in positions 5–9 of a Turkish IBAN (TRkk BBBBB 0 ...).
// Source: the EFT bank codes of the Banks Association of Türkiye (TBB).
internal sealed class TurkeyBanks : IBankDirectory
{
    public static TurkeyBanks Instance { get; } = new();

    public IReadOnlyList<Bank> All { get; } =
    [
        new("00001", "Türkiye Cumhuriyet Merkez Bankası"),
        new("00010", "T.C. Ziraat Bankası A.Ş."),
        new("00012", "Türkiye Halk Bankası A.Ş."),
        new("00015", "Türkiye Vakıflar Bankası T.A.O."),
        new("00032", "Türk Ekonomi Bankası A.Ş."),
        new("00046", "Akbank T.A.Ş."),
        new("00059", "Şekerbank T.A.Ş."),
        new("00062", "Türkiye Garanti Bankası A.Ş."),
        new("00064", "Türkiye İş Bankası A.Ş."),
        new("00067", "Yapı ve Kredi Bankası A.Ş."),
        new("00099", "ING Bank A.Ş."),
        new("00100", "Adabank A.Ş."),
        new("00103", "Fibabanka A.Ş."),
        new("00108", "Turkland Bank A.Ş."),
        new("00109", "ICBC Turkey Bank A.Ş."),
        new("00111", "QNB Finansbank A.Ş."),
        new("00123", "HSBC Bank A.Ş."),
        new("00124", "Alternatifbank A.Ş."),
        new("00125", "Burgan Bank A.Ş."),
        new("00134", "DenizBank A.Ş."),
        new("00135", "Anadolubank A.Ş."),
        new("00143", "Aktif Yatırım Bankası A.Ş."),
        new("00146", "Odea Bank A.Ş."),
        new("00203", "Albaraka Türk Katılım Bankası A.Ş."),
        new("00205", "Kuveyt Türk Katılım Bankası A.Ş."),
        new("00206", "Türkiye Finans Katılım Bankası A.Ş."),
        new("00209", "Ziraat Katılım Bankası A.Ş."),
        new("00210", "Vakıf Katılım Bankası A.Ş."),
        new("00211", "Türkiye Emlak Katılım Bankası A.Ş."),
        new("00212", "Hayat Finans Katılım Bankası A.Ş."),
    ];

    public Bank? FindByIban(string iban)
    {
        if (iban.Length < 9)
            return null;

        ReadOnlySpan<char> code = iban.AsSpan(4, 5);
        foreach (Bank bank in All)
        {
            if (code.SequenceEqual(bank.Code))
                return bank;
        }

        return null;
    }
}
