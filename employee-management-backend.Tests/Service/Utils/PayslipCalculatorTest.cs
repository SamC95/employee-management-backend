using employee_management_backend.Model;
using employee_management_backend.Service.Utils;
using employee_management_backend.Service.Utils.Calculators;

namespace employee_management_backend.Tests.Service.Utils;

public class PayslipCalculatorTest
{
    private readonly Payslip _testPayslip = new()
    {
        EmployeeId = "7734021",
        EmployeeName = "Hassan Patel",
        PayslipStartDate = new DateOnly(2025, 7, 1),
        PayslipEndDate = new DateOnly(2025, 7, 31),
        CompanyName = "Vertex Innovations Ltd",
        EmployeeDepartment = "Product Development",
        DaysWorkedPerWeek = 5,
        HoursWorked = 168,
        HolidayHours = 10,
        SickDates = [new DateOnly(2025, 7, 11)]
    };
    
    private readonly Employee _testEmployee = new()
    {
        EmployeeId = "7734021",
        ClockId = "9988776",
        FirstName = "Hassan",
        LastName = "Patel",
        Email = "hassan.patel@vertexinnovations.co.uk",
        PhoneNum = "07700 900234",
        Address = "42 Tech Park Avenue",
        City = "Manchester",
        PostCode = "M1 4AB",
        Country = "United Kingdom",
        Gender = "Male",
        DateOfBirth = new DateOnly(1990,
            3,
            14),
        DateHired = new DateOnly(2022,
            6,
            1),
        IsAdmin = false,
        IsManager = true,
        IsActive = true,
        JobTitle = "Product Developer",
        NationalInsuranceNumber = "BB1234567",
        NationalInsuranceCategory = "A",
        TaxCode = "1257L",
        HasPension = false,
        EmployeePensionContributionPercentage = 0,
        EmployerPensionContributionPercentage = 0,
        HasUnion = false,
        UnionContributionPercentage = 0,
        PayPerHour = 23.40m
    };

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
