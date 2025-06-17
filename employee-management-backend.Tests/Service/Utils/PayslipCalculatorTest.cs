using employee_management_backend.Model;
using employee_management_backend.Service.Utils;

namespace employee_management_backend.Tests.Service.Utils;

public class PayslipCalculatorTest
{
    private readonly Payslip _testPayslip = new()
    {
        EmployeeId = "7734021",
        EmployeeName = "Hassan Patel",
        NationalInsuranceNumber = "QQ112233A",
        NationalInsuranceCategory = "A",
        PayslipStartDate = new DateOnly(2025, 7, 1),
        PayslipEndDate = new DateOnly(2025, 7, 31),
        CompanyName = "Vertex Innovations Ltd",
        EmployeeDepartment = "Product Development",
        DaysWorkedPerWeek = 5,
        TaxCode = "1257L",
        HoursWorked = 168,
        HolidayHours = 10,
        HasPension = true,
        HasUnion = false,
        PensionContributionPercentage = 4,
        UnionContributionPercentage = 0,
        PayPerHour = 23.40m,
        SickDates = [new DateOnly(2025, 7, 11)]
    };

    [Fact]
    public void Calculate_GrossPayAndNetPayAndContributions_AreCorrect()
    {
        PayslipCalculator.Calculate(_testPayslip);

        var expectedGrossPay = (_testPayslip.HoursWorked + _testPayslip.HolidayHours) * _testPayslip.PayPerHour;
        Assert.Equal(expectedGrossPay, _testPayslip.GrossPay);

        var expectedPensionPay = expectedGrossPay * (_testPayslip.PensionContributionPercentage / 100.0m);
        Assert.Equal(expectedPensionPay, _testPayslip.EmployeePensionAmountPaid);
        Assert.Equal(expectedPensionPay, _testPayslip.EmployerPensionAmountPaid);

        var expectedUnionPay = expectedGrossPay * (_testPayslip.UnionContributionPercentage / 100.0m);
        Assert.Equal(expectedUnionPay, _testPayslip.EmployeeUnionAmountPaid);

        Assert.True(_testPayslip.TaxAmountPaid >= 0);
        Assert.True(_testPayslip.NationalInsuranceAmountPaid >= 0);

        var expectedNetPay = _testPayslip.GrossPay - (_testPayslip.TaxAmountPaid +
                                                      _testPayslip.EmployeePensionAmountPaid +
                                                      _testPayslip.NationalInsuranceAmountPaid +
                                                      _testPayslip.EmployeeUnionAmountPaid);
        Assert.Equal(expectedNetPay, _testPayslip.NetPay);
    }

    [Fact]
    public void Calculate_ShouldThrowArgumentException_WHenEndDateBeforeStartDate()
    {
        _testPayslip.PayslipStartDate = new DateOnly(2025, 6, 30);
        _testPayslip.PayslipEndDate = new DateOnly(2025, 6, 1);
        
        Assert.Throws<ArgumentException>(() => PayslipCalculator.Calculate(_testPayslip));
    }

    [Fact]
    public void Calculate_ShouldHandle_NoPensionOrUnionContributions()
    {
        _testPayslip.HasPension = false;
        _testPayslip.HasUnion = false;
        _testPayslip.PensionContributionPercentage = 0;
        _testPayslip.UnionContributionPercentage = 0;
        
        PayslipCalculator.Calculate(_testPayslip);
        
        Assert.Equal(0m, _testPayslip.EmployeePensionAmountPaid);
        Assert.Equal(0m, _testPayslip.EmployerPensionAmountPaid);
        Assert.Equal(0m, _testPayslip.EmployeeUnionAmountPaid);
    }
}
