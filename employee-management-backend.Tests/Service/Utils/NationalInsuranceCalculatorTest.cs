using employee_management_backend.Service.Utils;

namespace employee_management_backend.Tests.Service.Utils;

public class NationalInsuranceCalculatorTest
{
    [Fact]
    public void CalculateNationalInsurance_CategoryA_NoSickDays_ReturnsCorrectAmount()
    {
        var result =
            NationalInsuranceCalculator.CalculateNationalInsurance(
                968m * 4,
                4,
                [],
                "A",
                5);

        Assert.InRange(result, 232.0m, 232.1m);
    }

    [Fact]
    public void CalculateNationalInsurance_CategoryA_WithSickDays_AdjustsPayCorrectly()
    {
        var result =
            NationalInsuranceCalculator.CalculateNationalInsurance(
                968m * 4,
                4,
                new Dictionary<int, int> { { 1, 2 } },
                "A",
                5);

        Assert.InRange(result, 201.1m, 201.2m);
    }

    [Fact]
    public void CalculateNationalInsurance_CategoryJ_AboveUpperEarningsLimit_ReturnsDeferredNI()
    {
        var result =
            NationalInsuranceCalculator.CalculateNationalInsurance(
                5000m,
                4,
                [],
                "J",
                5
            );

        const decimal expectedUpperBandWeekly = 1250m - 967m;
        const decimal expectedWeeklyNationalInsurance = expectedUpperBandWeekly * 0.02m;
        const decimal expectedTotalNationalInsurance = expectedWeeklyNationalInsurance * 4;

        Assert.Equal(Math.Round(expectedTotalNationalInsurance, 2), result);
    }

    [Fact]
    public void CalculateNationalInsurance_CategoryB_ReturnsNoNationalInsurance()
    {
        var result = NationalInsuranceCalculator.CalculateNationalInsurance(
            4000m,
            4,
            [],
            "B",
            5
        );

        Assert.Equal(0m, result);
    }

    [Fact]
    public void CalculateNationalInsurance_InvalidDaysWorkedPerWeek_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            NationalInsuranceCalculator.CalculateNationalInsurance(
                3000m,
                4,
                [],
                "A",
                0
            ));

        Assert.Throws<ArgumentException>(() =>
            NationalInsuranceCalculator.CalculateNationalInsurance(
                3000m,
                4,
                [],
                "A",
                8
            ));
    }

    [Fact]
    public void CalculateNationalInsurance_UnsupportedCategory_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            NationalInsuranceCalculator.CalculateNationalInsurance(
                3000m,
                4,
                [],
                "X",
                5
            ));
        
        Assert.Contains("Unsupported NI category", exception.Message);
    }
}
