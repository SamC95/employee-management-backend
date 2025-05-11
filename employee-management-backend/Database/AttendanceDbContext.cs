using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Database;

public class EmployeeDatabase : DbContext
{
    public EmployeeDatabase(DbContextOptions<EmployeeDatabase> options) : base(options) { }
}