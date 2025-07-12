using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service.Interface;

namespace employee_management_backend.Service;

public class AnnouncementService(IAnnouncementRepository announcementRepository) : IAnnouncementService
{
    public async Task CreateAnnouncementPost(Announcement announcement)
    {
        if (announcement == null)
        {
            throw new ArgumentException("Announcement details are missing");
        }
        
        await announcementRepository.CreateAnnouncementPost(announcement);
    }
}
