using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service.Interface;

namespace employee_management_backend.Service;

public class ShiftService(IShiftRepository shiftRepository, IEmployeeRepository employeeRepository) : IShiftService
{
    public async Task AddWorkShift(WorkShift shift)
    {
        var employee = await employeeRepository.GetEmployeeById(shift.EmployeeId);

        if (employee == null)
        {
            throw new ArgumentException("Employee not found");
        }
        
        await shiftRepository.AddWorkShift(shift);
    }

    public async Task<bool> UpdateWorkShift(WorkShift shift)
    {
        return await shiftRepository.UpdateWorkShift(shift);
    }

    public async Task<bool> DeleteWorkShift(Guid shiftId)
    {
        return await shiftRepository.DeleteWorkShift(shiftId);
    }
    
    public async Task<WorkShift?> GetShiftById(Guid shiftId)
    {
        return await shiftRepository.GetShiftById(shiftId);
    }
}
