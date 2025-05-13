using employee_management_backend.Model;

using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Database;

public class AttendanceDbContext(DbContextOptions<AttendanceDbContext> options) : DbContext(options)
{
    // ReSharper disable once InconsistentNaming
    public DbSet<ClockEvent> attendance { get; set; }
}
