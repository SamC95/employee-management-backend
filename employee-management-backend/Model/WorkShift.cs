using System.ComponentModel.DataAnnotations;

namespace employee_management_backend.Model;

public class WorkShift
{
    [Key] 
    public Guid ShiftId { get; set; } = Guid.NewGuid();
    public required string? EmployeeId { get; set; }
    public required DateOnly? Date { get; set; }
    public required TimeOnly? StartTime { get; set; }
    public required TimeOnly? EndTime { get; set; }
}
