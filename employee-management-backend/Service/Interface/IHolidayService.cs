using employee_management_backend.Model;

namespace employee_management_backend.Service.Interface;

public interface IHolidayService
{
    Task CreateHolidayRequest(HolidayEvent holidayEvent);
    
    Task<bool> UpdateHolidayStatus(HolidayEvent holidayEvent);
    
    Task<List<HolidayEvent>> GetUpcomingHolidaysForLoggedInUser(string userId, int limit);
}
