namespace HeroKit.Turkish;

/// <summary>
/// A Turkish province with its plate code.
/// </summary>
/// <param name="Code">The province code from 1 to 81. This is also the first part of a vehicle plate.</param>
/// <param name="Name">The province name with Turkish letters.</param>
public readonly record struct Province(int Code, string Name);

/// <summary>
/// Provides the list of the 81 Turkish provinces and their plate codes.
/// </summary>
public static class TurkishProvinces
{
    /// <summary>
    /// All 81 provinces, ordered by code from 1 to 81.
    /// </summary>
    /// <example>
    /// <code>
    /// TurkishProvinces.All.Count; // 81
    /// TurkishProvinces.All[33].Name; // "İstanbul"
    /// </code>
    /// </example>
    public static IReadOnlyList<Province> All { get; } = new Province[]
    {
        new Province(1, "Adana"),
        new Province(2, "Adıyaman"),
        new Province(3, "Afyonkarahisar"),
        new Province(4, "Ağrı"),
        new Province(5, "Amasya"),
        new Province(6, "Ankara"),
        new Province(7, "Antalya"),
        new Province(8, "Artvin"),
        new Province(9, "Aydın"),
        new Province(10, "Balıkesir"),
        new Province(11, "Bilecik"),
        new Province(12, "Bingöl"),
        new Province(13, "Bitlis"),
        new Province(14, "Bolu"),
        new Province(15, "Burdur"),
        new Province(16, "Bursa"),
        new Province(17, "Çanakkale"),
        new Province(18, "Çankırı"),
        new Province(19, "Çorum"),
        new Province(20, "Denizli"),
        new Province(21, "Diyarbakır"),
        new Province(22, "Edirne"),
        new Province(23, "Elazığ"),
        new Province(24, "Erzincan"),
        new Province(25, "Erzurum"),
        new Province(26, "Eskişehir"),
        new Province(27, "Gaziantep"),
        new Province(28, "Giresun"),
        new Province(29, "Gümüşhane"),
        new Province(30, "Hakkari"),
        new Province(31, "Hatay"),
        new Province(32, "Isparta"),
        new Province(33, "Mersin"),
        new Province(34, "İstanbul"),
        new Province(35, "İzmir"),
        new Province(36, "Kars"),
        new Province(37, "Kastamonu"),
        new Province(38, "Kayseri"),
        new Province(39, "Kırklareli"),
        new Province(40, "Kırşehir"),
        new Province(41, "Kocaeli"),
        new Province(42, "Konya"),
        new Province(43, "Kütahya"),
        new Province(44, "Malatya"),
        new Province(45, "Manisa"),
        new Province(46, "Kahramanmaraş"),
        new Province(47, "Mardin"),
        new Province(48, "Muğla"),
        new Province(49, "Muş"),
        new Province(50, "Nevşehir"),
        new Province(51, "Niğde"),
        new Province(52, "Ordu"),
        new Province(53, "Rize"),
        new Province(54, "Sakarya"),
        new Province(55, "Samsun"),
        new Province(56, "Siirt"),
        new Province(57, "Sinop"),
        new Province(58, "Sivas"),
        new Province(59, "Tekirdağ"),
        new Province(60, "Tokat"),
        new Province(61, "Trabzon"),
        new Province(62, "Tunceli"),
        new Province(63, "Şanlıurfa"),
        new Province(64, "Uşak"),
        new Province(65, "Van"),
        new Province(66, "Yozgat"),
        new Province(67, "Zonguldak"),
        new Province(68, "Aksaray"),
        new Province(69, "Bayburt"),
        new Province(70, "Karaman"),
        new Province(71, "Kırıkkale"),
        new Province(72, "Batman"),
        new Province(73, "Şırnak"),
        new Province(74, "Bartın"),
        new Province(75, "Ardahan"),
        new Province(76, "Iğdır"),
        new Province(77, "Yalova"),
        new Province(78, "Karabük"),
        new Province(79, "Kilis"),
        new Province(80, "Osmaniye"),
        new Province(81, "Düzce"),
    };
}
