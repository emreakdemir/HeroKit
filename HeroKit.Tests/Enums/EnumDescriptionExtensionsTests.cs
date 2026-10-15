using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HeroKit.Enums;

namespace HeroKit.Tests.Enums;

public class EnumDescriptionExtensionsTests
{
    [Flags]
    public enum OrderStatus
    {
        [Description("Kargoya verildi")]
        Shipped = 1,

        [Display(Name = "Teslim edildi")]
        Delivered = 2,

        [Description("Description wins")]
        [Display(Name = "Display loses")]
        Both = 4,

        Plain = 8,
    }

    [Theory]
    [InlineData(OrderStatus.Shipped, "Kargoya verildi")]
    [InlineData(OrderStatus.Delivered, "Teslim edildi")]
    [InlineData(OrderStatus.Both, "Description wins")]
    [InlineData(OrderStatus.Plain, "Plain")]
    [InlineData(OrderStatus.Shipped | OrderStatus.Plain, "Shipped, Plain")]
    [InlineData((OrderStatus)64, "64")]
    public void GetDescription_ShouldPreferDescriptionThenDisplayThenName(OrderStatus value, string expected)
    {
        Assert.Equal(expected, value.GetDescription());
    }
}
