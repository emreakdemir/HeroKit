namespace HeroKit.Globalization;

/// <summary>
/// A second-level administrative unit of a country, such as a Turkish district (ilçe).
/// </summary>
/// <param name="Code">The official district code, for example the MERNİS code 1757 for Aladağ.</param>
/// <param name="Name">The name of the district in the country's language.</param>
/// <param name="RegionCode">The <see cref="Region.Code"/> of the region the district belongs to, for example 1 for Adana.</param>
public readonly record struct District(int Code, string Name, int RegionCode);
