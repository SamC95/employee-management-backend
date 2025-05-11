using employee_management_backend.Model;
using employee_management_backend.Repository;

namespace employee_management_backend.Service;

public class AttendanceService
{
    private readonly AttendanceRepository _repository;

    public AttendanceService(AttendanceRepository repository)
    {
        _repository = repository;
    }

    public async Task PostClockEvent(ClockEvent clockEvent)
    {
        clockEvent.Timestamp = clockEvent.Timestamp.ToUniversalTime();
        
        await _repository.SaveClockEvent(clockEvent);
    }

    public async Task<List<ClockEvent>> GetClockEventsByClockId(string clockId)
    {
        return await _repository.GetClockEventsByClockId(clockId);
    }
}
