using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service.Interface;

namespace employee_management_backend.Service;

public class ShiftService(IShiftRepository shiftRepository) : IShiftService
{
    public async Task AddWorkShift(WorkShift shift)
    {
        await shiftRepository.AddWorkShift(shift);
    }
}
