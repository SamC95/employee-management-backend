using employee_management_backend.Model;
using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Repository.Database;

public class AnnouncementDbContext(DbContextOptions<AnnouncementDbContext> options) : DbContext(options)
{
    public DbSet<Announcement> Announcements { get; set; }
}
