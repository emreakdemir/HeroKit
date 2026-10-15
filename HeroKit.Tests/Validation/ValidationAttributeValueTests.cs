using System.ComponentModel.DataAnnotations;
using HeroKit.Validation;

namespace HeroKit.Tests.Validation;

public class ValidationAttributeValueTests
{
    public static TheoryData<ValidationAttribute, string> ValidValues => new()
    {
        { new TcknAttribute(), "10000000146" },
        { new VknAttribute(), "4840847211" },
        { new TaxIdAttribute("TR"), "10000000146" },
        { new IbanAttribute(), "TR33 0006 1005 1978 6457 8413 26" },
        { new PhoneNumberAttribute("TR"), "0212 555 12 34" },
        { new PhoneNumberAttribute("TR") { MobileOnly = true }, "0532 123 45 67" },
        { new PostalCodeAttribute("TR"), "34710" },
        { new VehiclePlateAttribute("TR"), "34 ABC 123" },
        { new CardNumberAttribute(), "4111 1111 1111 1111" },
    };

    public static TheoryData<ValidationAttribute, string> InvalidValues => new()
    {
        { new TcknAttribute(), "10000000147" },
        { new VknAttribute(), "4840847212" },
        { new TaxIdAttribute("TR"), "123" },
        { new IbanAttribute(), "TR330006100519786457841327" },
        { new PhoneNumberAttribute("TR"), "0112 555 12 34" },
        { new PhoneNumberAttribute("TR") { MobileOnly = true }, "0212 555 12 34" },
        { new PostalCodeAttribute("TR"), "82000" },
        { new VehiclePlateAttribute("TR"), "34 Q 1234" },
        { new CardNumberAttribute(), "4111 1111 1111 1112" },
    };

    public static TheoryData<ValidationAttribute> AllAttributes => new()
    {
        new TcknAttribute(),
        new VknAttribute(),
        new TaxIdAttribute("TR"),
        new IbanAttribute(),
        new PhoneNumberAttribute("TR"),
        new PhoneNumberAttribute("TR") { MobileOnly = true },
        new PostalCodeAttribute("TR"),
        new VehiclePlateAttribute("TR"),
        new CardNumberAttribute(),
    };

    [Theory]
    [MemberData(nameof(ValidValues))]
    public void IsValid_ShouldReturnTrue_ForValidString(ValidationAttribute attribute, string value)
    {
        Assert.True(attribute.IsValid(value));
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public void IsValid_ShouldReturnFalse_ForInvalidString(ValidationAttribute attribute, string value)
    {
        Assert.False(attribute.IsValid(value));
    }

    [Theory]
    [MemberData(nameof(AllAttributes))]
    public void IsValid_ShouldReturnTrue_ForNull(ValidationAttribute attribute)
    {
        Assert.True(attribute.IsValid(null));
    }

    [Theory]
    [MemberData(nameof(AllAttributes))]
    public void IsValid_ShouldReturnTrue_ForEmptyString(ValidationAttribute attribute)
    {
        Assert.True(attribute.IsValid(string.Empty));
    }

    [Theory]
    [MemberData(nameof(AllAttributes))]
    public void IsValid_ShouldReturnFalse_ForNonStringValue(ValidationAttribute attribute)
    {
        Assert.False(attribute.IsValid(12345L));
    }

    [Fact]
    public void TurkishPhone_ShouldAcceptLandline_WhenMobileOnlyIsFalse()
    {
        Assert.True(new PhoneNumberAttribute("TR") { MobileOnly = false }.IsValid("0212 555 12 34"));
    }

    [Fact]
    public void TurkishPhone_ShouldRejectLandline_WhenMobileOnlyIsTrue()
    {
        Assert.False(new PhoneNumberAttribute("TR") { MobileOnly = true }.IsValid("0212 555 12 34"));
    }
}
