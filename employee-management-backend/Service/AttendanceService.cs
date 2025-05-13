using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service.Interface;

namespace employee_management_backend.Service;

public class AttendanceService(IAttendanceRepository attendanceRepository) : IAttendanceService
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
