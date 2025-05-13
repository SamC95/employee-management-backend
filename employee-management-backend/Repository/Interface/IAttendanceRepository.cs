using employee_management_backend.Model;

namespace employee_management_backend.Repository.Interface;

public interface IAttendanceRepository
{
    Task SaveClockEvent(ClockEvent clockEvent);
    
    Task<List<ClockEvent>> GetClockEventsByClockId(string clockId);
}
