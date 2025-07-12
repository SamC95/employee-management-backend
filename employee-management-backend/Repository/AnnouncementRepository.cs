using employee_management_backend.Model;
using employee_management_backend.Repository.Database;
using employee_management_backend.Repository.Interface;

namespace employee_management_backend.Repository;

public class AnnouncementRepository(AnnouncementDbContext context) : IAnnouncementRepository
{
    public async Task CreateAnnouncementPost(Announcement announcement)
    {
        try
        {
            await context.Announcements.AddAsync(announcement);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred adding announcement post to database: {ex.Message}");
        }
    }
}
