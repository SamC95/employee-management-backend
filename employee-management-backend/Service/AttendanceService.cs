using employee_management_backend.Model;
using employee_management_backend.Repository;

namespace employee_management_backend.Service;

public class AttendanceService(AttendanceRepository attendanceRepository)
{
    public async Task PostClockEvent(ClockEvent clockEvent)
    {
        clockEvent.Timestamp = clockEvent.Timestamp.ToUniversalTime();
        
        await attendanceRepository.SaveClockEvent(clockEvent);
    }

    public async Task<List<ClockEvent>> GetClockEventsByClockId(string clockId)
    {
        return await attendanceRepository.GetClockEventsByClockId(clockId);
    }
}
