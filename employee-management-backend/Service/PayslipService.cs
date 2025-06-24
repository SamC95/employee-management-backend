using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service.Interface;
using employee_management_backend.Service.Utils.Calculators;

namespace employee_management_backend.Service;

public class PayslipService(IPayslipRepository payslipRepository, IEmployeeRepository employeeRepository)
    : IPayslipService
{
    public async Task CreatePayslip(Payslip payslip)
    {
        ArgumentNullException.ThrowIfNull(payslip);
        
        var employee = await employeeRepository.GetEmployeeById(payslip.EmployeeId);

        if (employee == null)
            throw new ArgumentException("Employee not found");
        
        if (payslip.DaysWorkedPerWeek <= 0)
            throw new ArgumentException("Days worked per week must be greater than 0");

        if (payslip.HoursWorked < 0 || payslip.HolidayHours < 0)
            throw new ArgumentException("Hours worked or holiday hours cannot be a negative number");

        if (payslip.PayslipStartDate >= payslip.PayslipEndDate)
            throw new ArgumentException("Payslip start date cannot be after or equal to the payslip end date");

        switch (employee)
        {
            case { HasPension: false, EmployeePensionContributionPercentage: > 0 }:
                throw new ArgumentException(
                    "Employee pension contribution percentage must be 0 if employee does not have a pension");
            case { HasPension: false, EmployerPensionContributionPercentage: > 0 }:
                throw new ArgumentException(
                    "Employer pension contribution percentage must be 0 if employee does not have a pension");
            case { HasUnion: false, UnionContributionPercentage: > 0 }:
                throw new ArgumentException("Union contribution percentage must be 0 if employee does not have an union");
        }

        PayslipCalculator.Calculate(payslip, employee);

        await payslipRepository.CreatePayslip(payslip);
    }
}
