using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;

namespace employee_management_backend.Service;

public class AttendanceService
{
    private readonly IAttendanceRepository _repository;

    public AttendanceService(IAttendanceRepository repository)
    {
        _repository = repository;
    }

    public async Task PostClockEvent(ClockEvent clockEvent)
    {
        clockEvent.Timestamp = clockEvent.Timestamp.ToUniversalTime();
        
        await _repository.SaveClockEvent(clockEvent);
    }
}
