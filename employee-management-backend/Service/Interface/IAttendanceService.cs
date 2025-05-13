using employee_management_backend.Model;

namespace employee_management_backend.Service.Interface;

public interface IAttendanceService
{
    Task PostClockEvent(ClockEvent clockEvent);
    
    Task<List<ClockEvent>> GetClockEventsByClockId(string clockId);
}
