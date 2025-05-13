using employee_management_backend.Model;

namespace employee_management_backend.Service.Interface;

public interface IEmployeeService
{
    Task CreateEmployee(Employee employee);
    
    Task<Employee?> GetEmployeeById(string employeeId);
    
    Task<bool?> CheckClockIdExists(string clockId);
}
