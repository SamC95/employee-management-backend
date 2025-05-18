using employee_management_backend.Model;
using employee_management_backend.Repository.Database;
using employee_management_backend.Repository.Interface;

namespace employee_management_backend.Repository;

public class ShiftRepository(ShiftDbContext context) : IShiftRepository
{
    public async Task AddWorkShift(WorkShift shift)
    {
        try
        {
            await context.Shifts.AddAsync(shift);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred adding work shift to database: {ex.Message}");
        }
    }
}
