using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service.Interface;
using employee_management_backend.Service.Utils;

namespace employee_management_backend.Service;

public class PayslipService(IPayslipRepository payslipRepository, IEmployeeRepository employeeRepository) : IPayslipService
{
    public async Task CreatePayslip(Payslip payslip)
    {
        ArgumentNullException.ThrowIfNull(payslip);

        if (payslip.DaysWorkedPerWeek <= 0)
            throw new ArgumentException("Days worked per week must be greater than 0");
        
        var employee = await employeeRepository.GetEmployeeById(payslip.EmployeeId);

        if (employee == null)
            throw new ArgumentException("Employee not found");
        
        PayslipCalculator.Calculate(payslip);
        
        await payslipRepository.CreatePayslip(payslip);
    }
}