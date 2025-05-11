using employee_management_backend.Database;
using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;

namespace employee_management_backend.Repository;

public class AttendanceRepository(AttendanceDbContext context) : IAttendanceRepository
{
    public async Task SaveClockEvent(ClockEvent clockEvent)
    {
        try
        {
            await context.attendance.AddAsync(clockEvent);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
            throw;
        }
    }
}
