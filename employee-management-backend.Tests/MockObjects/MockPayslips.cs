using employee_management_backend.Model;

namespace employee_management_backend.Tests.MockObjects;

public static class MockPayslips
{
    internal static Payslip CreatePayslip() => new()
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

    internal static Payslip CreateCalculatedPayslip() => new()
    {
        PayslipId = Guid.NewGuid(),
        EmployeeId = "7734021",
        EmployeeName = "Hassan Patel",
        PayslipStartDate = new DateOnly(2025, 7, 1),
        PayslipEndDate = new DateOnly(2025, 7, 31),
        CompanyName = "Vertex Innovations Ltd",
        EmployeeDepartment = "Product Development",
        DaysWorkedPerWeek = 5,
        HoursWorked = 168,
        HolidayHours = 10,
        SickDates = [new DateOnly(2025, 7, 11)],
        GrossPay = 3931.20m,
        TaxAmountPaid = 388.20m,
        EmployeePensionAmountPaid = 157.25m,
        EmployerPensionAmountPaid = 157.25m,
        EmployeeUnionAmountPaid = 0m,
        NationalInsuranceAmountPaid = 313.50m,
        NetPay = 3155.00m
    };
}
