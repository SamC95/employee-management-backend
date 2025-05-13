using employee_management_backend.Database;
using employee_management_backend.Model;

namespace employee_management_backend.Repository;

public class EmployeeRepository(EmployeeDbContext context)
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
}
