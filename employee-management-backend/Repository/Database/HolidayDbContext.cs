using employee_management_backend.Model;
using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Repository.Database;

public class HolidayDbContext(DbContextOptions<HolidayDbContext> options) : DbContext(options)
{
    public DbSet<HolidayEvent> Holidays { get; set; }
}
