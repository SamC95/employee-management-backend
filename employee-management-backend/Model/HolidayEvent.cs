using System.ComponentModel.DataAnnotations;

namespace employee_management_backend.Model;

public class HolidayEvent
{
    [Key]
    public Guid EventId { get; set; } = Guid.NewGuid();
    
    [MaxLength(20)]
    public required string? EmployeeId { get; set; }
    
    public required DateOnly Date { get; set; }
    
    [MaxLength(50)]
    public required string? Status { get; set; }
    
    [MaxLength(20)]
    // Optional id for if assigning an employee's holiday management to a specific manager
    public string? ManagerId { get; set; } 
}
