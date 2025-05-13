using employee_management_backend.Model;

namespace employee_management_backend.Repository.Interface;

public interface IEmployeeRepository
{
    Task CreateEmployee(Employee employee);
    
    Task<Employee?> GetEmployeeById(string employeeId);
}
