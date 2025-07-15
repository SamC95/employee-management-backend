using employee_management_backend.Model;
using employee_management_backend.Repository.Database;
using employee_management_backend.Repository.Interface;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<Announcement>> GetRecentAnnouncements(List<string> allowedAudiences, int amountToRetrieve)
    {
        try
        {
            return await context.Announcements
                .Where(announcement =>
                    announcement.Audience != null && allowedAudiences.Contains(announcement.Audience))
                .OrderByDescending(announcement => announcement.CreationDate)
                .Take(amountToRetrieve)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred whilst retrieving announcement posts: {ex.Message}");
        }
    }
}
