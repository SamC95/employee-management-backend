using employee_management_backend.Model;

namespace employee_management_backend.Service.Interface;

public interface IAnnouncementService
{
    Task CreateAnnouncementPost(Announcement announcement);
    
    Task<List<Announcement>> GetRecentAnnouncements(string userId);
}
