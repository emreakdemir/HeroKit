using FluentValidation;
using FluentValidation.Results;
using HeroKit.FluentValidation;

namespace HeroKit.Tests.FluentValidation;

public class HeroKitRuleBuilderExtensionsTests
{
    public sealed class Customer
    {
        // Non-nullable on purpose: the rules must compile for string and string? without warnings.
        public string IdentityNumber { get; set; } = "";
        public string? TaxNumber { get; set; }
        public string? TaxId { get; set; }
        public string? Iban { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? PostalCode { get; set; }
        public string? Plate { get; set; }
        public string? Card { get; set; }
    }

    private sealed class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.IdentityNumber).NotEmpty().Tckn();
            RuleFor(x => x.TaxNumber).Vkn();
            RuleFor(x => x.TaxId).TaxIdTr();
            RuleFor(x => x.Iban).Iban();
            RuleFor(x => x.Phone).TurkishPhone();
            RuleFor(x => x.Mobile).TurkishPhone(mobileOnly: true).WithMessage("Cep telefonu geçersiz.");
            RuleFor(x => x.PostalCode).PostalCodeTr();
            RuleFor(x => x.Plate).PlateTr();
            RuleFor(x => x.Card).CardNumber();
        }
    }

    private static readonly CustomerValidator Validator = new();

    private static Customer Valid() => new()
    {
        IdentityNumber = "10000000146",
        TaxNumber = "4840847211",
        TaxId = "4840847211",
        Iban = "TR33 0006 1005 1978 6457 8413 26",
        Phone = "0212 555 12 34",
        Mobile = "0532 123 45 67",
        PostalCode = "34710",
        Plate = "34 ABC 123",
        Card = "4111 1111 1111 1111",
    };

    [Fact]
    public void ValidCustomer_ShouldPass()
    {
        Assert.True(Validator.Validate(Valid()).IsValid);
    }

    [Fact]
    public void EmptyOptionalValues_ShouldPass()
    {
        Assert.True(Validator.Validate(new Customer { IdentityNumber = "10000000146", Iban = "", Card = null }).IsValid);
    }

    [Fact]
    public void InvalidValues_ShouldFailWithPropertyMessages()
    {
        Customer customer = Valid();
        customer.IdentityNumber = "10000000147";
        customer.TaxNumber = "1";
        customer.TaxId = "123";
        customer.Iban = "TR330006100519786457841327";
        customer.Phone = "0112 555 12 34";
        customer.Mobile = "0212 555 12 34";
        customer.PostalCode = "82000";
        customer.Plate = "34 Q 1234";
        customer.Card = "4111 1111 1111 1112";

        ValidationResult result = Validator.Validate(customer);

        Assert.Equal(9, result.Errors.Count);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "'Identity Number' must be a valid Turkish identity number (TCKN).");
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Cep telefonu geçersiz.");
    }
}
