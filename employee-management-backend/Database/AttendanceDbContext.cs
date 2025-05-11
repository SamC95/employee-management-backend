using employee_management_backend.Model;
using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Database;

public class AttendanceDbContext : DbContext
{
    // ReSharper disable once InconsistentNaming
    public DbSet<ClockEvent> attendance { get; set; }

    public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options) : base(options)
    {
    }
}
