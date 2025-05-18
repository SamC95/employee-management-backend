using employee_management_backend.Model;
using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Repository.Database;

public class ShiftDbContext(DbContextOptions<ShiftDbContext> options) : DbContext(options)
{
    public DbSet<WorkShift> Shifts { get; set; }
}
