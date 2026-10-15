using System.ComponentModel.DataAnnotations;
using HeroKit.Validation;

namespace HeroKit.Tests.Validation;

public class ValidationAttributeTests
{
    private sealed class Customer
    {
        [Required, Tckn]
        public string? IdentityNumber { get; set; }

        [Vkn]
        public string? TaxNumber { get; set; }

        [TaxId("TR")]
        public string? TaxId { get; set; }

        [Iban]
        public string? Iban { get; set; }

        [PhoneNumber("TR")]
        public string? Phone { get; set; }

        [PhoneNumber("TR", MobileOnly = true, ErrorMessage = "Cep telefonu geçersiz.")]
        public string? Mobile { get; set; }

        [PostalCode("TR")]
        public string? PostalCode { get; set; }

        [VehiclePlate("TR")]
        public string? Plate { get; set; }

        [CardNumber]
        public string? Card { get; set; }
    }

    private static List<ValidationResult> Validate(Customer customer)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(customer, new ValidationContext(customer), results, validateAllProperties: true);
        return results;
    }

    private static Customer Valid() => new()
    {
        IdentityNumber = "10000000146",
        TaxNumber = "4840847211",
        TaxId = "10000000146",
        Iban = "TR33 0006 1005 1978 6457 8413 26",
        Phone = "0212 555 12 34",
        Mobile = "0532 123 45 67",
        PostalCode = "34710",
        Plate = "34 ABC 123",
        Card = "4111 1111 1111 1111",
    };

    [Fact]
    public void ValidModel_ShouldPass()
    {
        Assert.Empty(Validate(Valid()));
    }

    [Fact]
    public void EmptyOptionalFields_ShouldPass()
    {
        Assert.Empty(Validate(new Customer { IdentityNumber = "10000000146", TaxNumber = "", Iban = null }));
    }

    [Fact]
    public void InvalidFields_ShouldReportEachMember()
    {
        Customer customer = Valid();
        customer.IdentityNumber = "10000000147";
        customer.TaxNumber = "4840847212";
        customer.TaxId = "123";
        customer.Iban = "TR330006100519786457841327";
        customer.Phone = "0112 555 12 34";
        customer.Mobile = "0212 555 12 34";
        customer.PostalCode = "82000";
        customer.Plate = "34 Q 1234";
        customer.Card = "4111 1111 1111 1112";

        List<ValidationResult> results = Validate(customer);

        Assert.Equal(9, results.Count);
        Assert.Contains(results, r => r.MemberNames.Single() == nameof(Customer.IdentityNumber)
                                      && r.ErrorMessage == "The IdentityNumber field must be a valid Turkish identity number (TCKN).");
        Assert.Contains(results, r => r.ErrorMessage == "Cep telefonu geçersiz.");
    }

    [Fact]
    public void NonStringValues_ShouldBeInvalid()
    {
        Assert.False(new TcknAttribute().IsValid(10000000146L));
        Assert.True(new TcknAttribute().IsValid(null));
    }
}
