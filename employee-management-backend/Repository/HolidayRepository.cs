using employee_management_backend.Model;
using employee_management_backend.Repository.Database;
using employee_management_backend.Repository.Interface;

namespace employee_management_backend.Repository;

public class HolidayRepository(HolidayDbContext context) : IHolidayRepository
{
    public async Task CreateHolidayRequest(HolidayEvent holidayEvent)
    {
        try
        {
            await context.Holidays.AddAsync(holidayEvent);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occurred adding holiday event to database: {ex.Message}");
        }
    }

    public Task<bool> UpdateHolidayStatus(HolidayEvent holidayEvent)
    {
        var existingHolidayRequest = context.Holidays.Find(holidayEvent.EventId);

        if (existingHolidayRequest == null)
        {
            throw new Exception($"Holiday event with id {holidayEvent.EventId} does not exist");
        }
        
        if (holidayEvent.Status is not null) existingHolidayRequest.Status = holidayEvent.Status;

        context.SaveChanges();
        return Task.FromResult(true);
    }
}
