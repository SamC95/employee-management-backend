using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service.Interface;

namespace employee_management_backend.Service;

public class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
{
    public async Task CreateEmployee(Employee employee)
    {
        await employeeRepository.CreateEmployee(employee);
    }

    public async Task<Employee?> GetEmployeeById(string employeeId)
    {
        return await employeeRepository.GetEmployeeById(employeeId);
    }

    public async Task<bool?> CheckClockIdExists(string clockId)
    {
        return await employeeRepository.CheckClockIdExists(clockId);
    }
}
