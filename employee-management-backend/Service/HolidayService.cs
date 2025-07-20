using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service.Interface;

namespace employee_management_backend.Service;

public class HolidayService(IHolidayRepository holidayRepository, IEmployeeRepository employeeRepository) : IHolidayService
{
    public async Task CreateHolidayRequest(HolidayEvent holidayEvent)
    {
        var employee = await employeeRepository.GetEmployeeById(holidayEvent.EmployeeId);

        if (employee == null)
        {
            throw new ArgumentException("No employee found by id number " + holidayEvent.EmployeeId);
        }
        
        await holidayRepository.CreateHolidayRequest(holidayEvent);
    }

    public async Task<bool> UpdateHolidayStatus(HolidayEvent holidayEvent)
    {
        return await holidayRepository.UpdateHolidayStatus(holidayEvent);
    }

    public async Task<List<HolidayEvent>> GetUpcomingHolidaysForLoggedInUser(string userId, int limit)
    {
        return await holidayRepository.GetUpcomingHolidaysForLoggedInUser(userId, limit);
    }
}
