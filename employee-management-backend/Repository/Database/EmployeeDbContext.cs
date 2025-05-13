using employee_management_backend.Model;
using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Database;

public class EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }
}
