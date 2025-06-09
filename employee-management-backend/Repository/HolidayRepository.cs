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
            Console.WriteLine($"Error occurred adding holiday event to database: {ex.Message}");
        }
    }
}
