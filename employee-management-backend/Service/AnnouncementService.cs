using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service.Interface;

namespace employee_management_backend.Service;

public class AnnouncementService(IAnnouncementRepository announcementRepository, IEmployeeRepository employeeRepository)
    : IAnnouncementService
{
    public async Task CreateAnnouncementPost(Announcement announcement)
    {
        if (announcement == null)
        {
            throw new ArgumentException("Announcement details are missing");
        }

        await announcementRepository.CreateAnnouncementPost(announcement);
    }

    public async Task<List<Announcement>> GetRecentAnnouncements(string userId)
    {
        var employee = await employeeRepository.GetEmployeeById(userId);
        if (employee == null)
            throw new ArgumentException("Employee not found");

        var allowedAudiences =
            employee.IsAdmin ? new List<string> { "All-Staff", "Management", "Admin" } :
            employee.IsManager ? new List<string> { "All-Staff", "Management" } :
            new List<string> { "All-Staff" };

        return await announcementRepository.GetRecentAnnouncements(allowedAudiences, 5);
    }
}
