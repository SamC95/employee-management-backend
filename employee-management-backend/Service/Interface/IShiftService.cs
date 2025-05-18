using employee_management_backend.Model;

namespace employee_management_backend.Service.Interface;

public interface IShiftService
{
    Task AddWorkShift(WorkShift shift);
}
