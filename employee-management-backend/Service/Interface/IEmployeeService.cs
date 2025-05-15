using employee_management_backend.Model;

namespace employee_management_backend.Service.Interface;

public interface IEmployeeService
{
    Task CreateEmployee(Employee employee);
    
    Task<bool> UpdateEmployeeDetails(EmployeeUpdater patch);
    
    Task<Employee?> GetEmployeeById(string employeeId);
    
    Task<List<Employee>> GetEmployeesByJobTitle(string jobTitle);
    
    Task<bool?> CheckClockIdExists(string clockId);
}
