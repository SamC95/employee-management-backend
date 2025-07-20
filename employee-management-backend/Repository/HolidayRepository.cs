using employee_management_backend.Model;
using employee_management_backend.Repository.Database;
using employee_management_backend.Repository.Interface;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<HolidayEvent>> GetUpcomingHolidaysForLoggedInUser(string userId, int amountToRetrieve)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        
        try
        {
            return await context.Holidays
                .Where(holiday => holiday.EmployeeId == userId && holiday.Date >= today)
                .OrderBy(holiday => holiday.Date)
                .Take(amountToRetrieve)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occurred getting holidays for logged in user: {ex.Message}");
        }
    }
}
