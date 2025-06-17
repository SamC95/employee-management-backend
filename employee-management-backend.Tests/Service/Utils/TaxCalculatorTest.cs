using employee_management_backend.Service.Utils;

namespace employee_management_backend.Tests.Service.Utils;

public class TaxCalculatorTest
{
    private const decimal GrossPay = 2000m;
    
    [Fact]
    public void CalculateTax_TaxCode_BR_Returns20PercentOfGrossPay()
    {
        var result = TaxCalculator.CalculateTax(GrossPay, "BR");
        
        Assert.Equal(400m, result);
    }

    [Fact]
    public void CalculateTax_TaxCode_D0_Returns40PercentOfGrossPay()
    {
        var result = TaxCalculator.CalculateTax(GrossPay, "D0");
        
        Assert.Equal(800m, result);
    }

    [Fact]
    public void CalculateTax_TaxCode_D1_Returns45PercentOfGrossPay()
    {
        var result = TaxCalculator.CalculateTax(GrossPay, "D1");
        
        Assert.Equal(900m, result);
    }

    [Fact]
    public void CalculateTax_TaxCode_NT_ReturnsZero()
    {
        var result = TaxCalculator.CalculateTax(GrossPay, "NT");
        
        Assert.Equal(0m, result);
    }

    [Fact]
    public void CalculateTax_TaxCode_1257L_BelowPersonalAllowance_ReturnsZero()
    {
        var result = TaxCalculator.CalculateTax(800m, "1257L");
        
        Assert.Equal(0m, result);
    }

    [Fact]
    public void CalculateTax_TaxCode_1257L_BasicRateOnly_ReturnsCorrectAmount()
    {
        var result = TaxCalculator.CalculateTax(1666.67m, "1257L");
        
        Assert.InRange(result, 123.8m, 123.9m);
    }

    [Fact]
    public void CalculateTax_TaxCode_1257L_BasicAndHigherRate_ReturnsCorrectAmount()
    {
        var result = TaxCalculator.CalculateTax(5000m, "1257L");
        
        Assert.InRange(result, 952.6m, 952.7m);
    }

    [Fact]
    public void CalculateTax_TaxCode_1257L_AllRates_ReturnsCorrectAmount()
    {
        var result = TaxCalculator.CalculateTax(12500m, "1257L");
        
        Assert.InRange(result, 4056.2m, 4056.3m);
    }

    [Fact]
    public void CalculateTax_UnknownTaxCode_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => TaxCalculator.CalculateTax(GrossPay, "XYZ"));
        
        Assert.Contains("Unsupported or unknown tax code", exception.Message);
    }

    [Fact]
    public void CalculateTax_TaxCode_1257L_LowPay_CustomPeriodsPerYearAppliesCorrectly()
    {
        var result = TaxCalculator.CalculateTax(GrossPay, "1257L", periodsPerYear: 4);
        
        Assert.Equal(0m, result);
    }
    
    [Fact]
    public void CalculateTax_TaxCode_1257L_HighPay_CustomPeriodsPerYearAppliesCorrectly()
    {
        var result = TaxCalculator.CalculateTax(40000m, "1257L", periodsPerYear: 4);
        
        Assert.InRange(result, 13293.7m, 13293.8m);
    }
}
