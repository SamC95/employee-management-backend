using employee_management_backend.Model;
using employee_management_backend.Repository.Database;
using employee_management_backend.Repository.Interface;
using Microsoft.EntityFrameworkCore;

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

    public Task<bool> UpdateWorkShift(WorkShift shift)
    {
        var existingShift = context.Shifts.Find(shift.ShiftId);

        if (existingShift == null)
        {
            throw new Exception($"Shift with ID {shift.ShiftId} was not found");
        }
        
        if (shift.EmployeeId is not null) existingShift.EmployeeId = shift.EmployeeId;
        if (shift.Date is not null) existingShift.Date = shift.Date;
        if (shift.StartTime is not null) existingShift.StartTime = shift.StartTime;
        if (shift.EndTime is not null) existingShift.EndTime = shift.EndTime;
        
        context.SaveChanges();
        return Task.FromResult(true);
    }

    public async Task<bool> DeleteWorkShift(Guid shiftId)
    {
        var shift = await context.Shifts.FindAsync(shiftId);

        if (shift == null) return false;
        
        context.Shifts.Remove(shift);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<WorkShift?> GetShiftById(Guid shiftId)
    {
        return await context.Shifts.FirstOrDefaultAsync(shift => shift.ShiftId == shiftId);
    }
}
