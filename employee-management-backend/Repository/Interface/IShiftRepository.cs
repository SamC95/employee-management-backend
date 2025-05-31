using employee_management_backend.Model;

namespace employee_management_backend.Repository.Interface;

public interface IShiftRepository
{
    Task AddWorkShift(WorkShift shift);
    
    Task<bool> UpdateWorkShift(WorkShift shift);
    
    Task<bool> DeleteWorkShift(Guid shiftId);
    
    Task<WorkShift?> GetShiftById(Guid shiftId);
}
