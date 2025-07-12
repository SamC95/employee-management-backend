using employee_management_backend.Model;
using employee_management_backend.Service.Utils.Calculators;
using static employee_management_backend.Tests.MockObjects.MockEmployees;
using static employee_management_backend.Tests.MockObjects.MockPayslips;

namespace employee_management_backend.Tests.Service.Utils;

public class PayslipCalculatorTest
{
    private readonly Payslip _testPayslip = CreatePayslip();
    private readonly Employee _testEmployee = TestEmployeeHassan ;
    
    [Fact]
    public void Calculate_GrossPayAndNetPayAndContributions_AreCorrect()
    {
        PayslipCalculator.Calculate(_testPayslip, _testEmployee);

        var expectedGrossPay = (_testPayslip.HoursWorked + _testPayslip.HolidayHours) * _testEmployee.PayPerHour;
        Assert.Equal(expectedGrossPay, _testPayslip.GrossPay);

        var expectedPensionPay = expectedGrossPay * (_testEmployee.EmployeePensionContributionPercentage / 100.0m);
        Assert.Equal(expectedPensionPay, _testPayslip.EmployeePensionAmountPaid);
        Assert.Equal(expectedPensionPay, _testPayslip.EmployerPensionAmountPaid);

        var expectedUnionPay = expectedGrossPay * (_testEmployee.UnionContributionPercentage / 100.0m);
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
        
        Assert.Throws<ArgumentException>(() => PayslipCalculator.Calculate(_testPayslip, _testEmployee));
    }

    [Fact]
    public void Calculate_ShouldHandle_NoPensionOrUnionContributions()
    {
        _testEmployee.HasPension = false;
        _testEmployee.HasUnion = false;
        _testEmployee.EmployeePensionContributionPercentage = 0;
        _testEmployee.UnionContributionPercentage = 0;
        
        PayslipCalculator.Calculate(_testPayslip, _testEmployee);
        
        Assert.Equal(0m, _testPayslip.EmployeePensionAmountPaid);
        Assert.Equal(0m, _testPayslip.EmployerPensionAmountPaid);
        Assert.Equal(0m, _testPayslip.EmployeeUnionAmountPaid);
    }
}
