using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service.Interface;
using employee_management_backend.Service.Utils;

namespace employee_management_backend.Service;

public class PayslipService(IPayslipRepository payslipRepository, IEmployeeRepository employeeRepository)
    : IPayslipService
{
    public async Task CreatePayslip(Payslip payslip)
    {
        ArgumentNullException.ThrowIfNull(payslip);

        if (payslip.DaysWorkedPerWeek <= 0)
            throw new ArgumentException("Days worked per week must be greater than 0");

        if (payslip.HoursWorked < 0 || payslip.HolidayHours < 0)
            throw new ArgumentException("Hours worked or holiday hours cannot be a negative number");

        if (payslip.PayslipStartDate >= payslip.PayslipEndDate)
            throw new ArgumentException("Payslip start date cannot be after or equal to the payslip end date");

        switch (payslip)
        {
            case { HasPension: false, PensionContributionPercentage: > 0 }:
                throw new ArgumentException(
                    "Pension contribution percentage must be 0 if employee does not have a pension");
            case { HasUnion: false, UnionContributionPercentage: > 0 }:
                throw new ArgumentException("Union contribution percentage must be 0 if employee does not have an union");
        }

        var employee = await employeeRepository.GetEmployeeById(payslip.EmployeeId);

        if (employee == null)
            throw new ArgumentException("Employee not found");

        PayslipCalculator.Calculate(payslip);

        await payslipRepository.CreatePayslip(payslip);
    }
}