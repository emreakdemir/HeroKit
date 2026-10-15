namespace HeroKit.Banking;

/// <summary>
/// Identifies the brand (network) of a payment card.
/// </summary>
public enum CardBrand
{
    /// <summary>The brand is not known.</summary>
    Unknown,

    /// <summary>Visa.</summary>
    Visa,

    /// <summary>Mastercard.</summary>
    Mastercard,

    /// <summary>American Express.</summary>
    AmericanExpress,

    /// <summary>Discover.</summary>
    Discover,

    /// <summary>JCB.</summary>
    Jcb,

    /// <summary>Troy, the Turkish domestic card scheme.</summary>
    Troy,
}
