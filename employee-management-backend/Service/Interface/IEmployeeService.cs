using employee_management_backend.Model;

namespace employee_management_backend.Service.Interface;

public interface IEmployeeService
{
    Task<string> CreateEmployee(Employee employee);
    
    Task<bool> UpdateEmployeeDetails(EmployeeUpdater patch);
    
    Task<bool> DeleteEmployee(string employeeId);
    
    Task<Employee?> GetEmployeeById(string employeeId);
    
    Task<List<Employee>> GetEmployeesByJobTitle(string jobTitle);
    
    Task<bool?> CheckClockIdExists(string clockId);
}
