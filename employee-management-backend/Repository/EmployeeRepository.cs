using employee_management_backend.Database;
using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Repository;

public class EmployeeRepository(EmployeeDbContext context) : IEmployeeRepository
{
    public async Task CreateEmployee(Employee employee)
    {
        try
        {
            await context.Employees.AddAsync(employee);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred adding employee to database: {ex.Message}");
            throw;
        }
    }

    public async Task<Employee?> GetEmployeeById(string employeeId)
    {
        return await context.Employees
            .FirstOrDefaultAsync(employee => employee.EmployeeId == employeeId);
    }
    
    public async Task<bool?> CheckClockIdExists(string clockId)
    {
        return await context.Employees.AnyAsync(employee => employee.ClockId == clockId);
    }
}
