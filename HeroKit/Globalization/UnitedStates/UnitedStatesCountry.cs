namespace HeroKit.Globalization.UnitedStates;

// Vehicle plates are issued by each state in different formats, so the United States has no plate rules.
internal static class UnitedStatesCountry
{
    public static Country Create() => new("US", "United States", Languages.English)
    {
        Currency = Currencies.USDollar,
        PhoneNumbers = UnitedStatesPhoneNumberRules.Instance,
        PostalCodes = UnitedStatesPostalCodeRules.Instance,
        NationalIds = UnitedStatesNationalIdRules.Instance,
        TaxIds = UnitedStatesTaxIdRules.Instance,
        Holidays = UnitedStatesHolidayCalendar.Instance,
        Regions = UnitedStatesStates.All,
    };
}
