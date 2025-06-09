using employee_management_backend.Model;

namespace employee_management_backend.Service.Interface;

public interface IHolidayService
{
    Task CreateHolidayRequest(HolidayEvent holidayEvent);
}
