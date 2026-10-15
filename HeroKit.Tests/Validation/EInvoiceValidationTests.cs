using System.ComponentModel.DataAnnotations;
using FluentValidation;
using HeroKit.FluentValidation;
using HeroKit.Turkish;
using HeroKit.Validation;

namespace HeroKit.Tests.Validation;

// HeroKit.Turkish (InvoiceNumberTr struct) and HeroKit.Validation (InvoiceNumberTrAttribute) are both imported on purpose:
// [InvoiceNumberTr] must bind to the attribute.
public class EInvoiceValidationTests
{
    public sealed class Invoice
    {
        [InvoiceNumberTr]
        public string? Number { get; set; }

        [Ettn]
        public string? Ettn { get; set; }
    }

    private sealed class InvoiceValidator : AbstractValidator<Invoice>
    {
        public InvoiceValidator()
        {
            RuleFor(x => x.Number).InvoiceNumberTr();
            RuleFor(x => x.Ettn).Ettn();
        }
    }

    [Theory]
    [InlineData("ABC2026000000001", "51d3bb18-9650-4c8e-825d-5a7f845be630", 0)]
    [InlineData("ABC2026000000000", "not-a-uuid", 2)]
    [InlineData(null, "", 0)]
    public void AttributesAndRules_ShouldAgree(string? number, string? ettn, int errors)
    {
        var invoice = new Invoice { Number = number, Ettn = ettn };
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(invoice, new ValidationContext(invoice), results, validateAllProperties: true);

        Assert.Equal(errors, results.Count);
        Assert.Equal(errors, new InvoiceValidator().Validate(invoice).Errors.Count);
        Assert.Equal(number is not null && InvoiceNumberTr.TryParse(number, out _), number.IsValidInvoiceNumberTr());
    }
}
