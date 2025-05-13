using employee_management_backend.Model;
using employee_management_backend.Repository;

namespace employee_management_backend.Service;

public class EmployeeService(EmployeeRepository employeeRepository)
{
    public async Task CreateEmployee(Employee employee)
    {
        await employeeRepository.CreateEmployee(employee);
    }   
}
