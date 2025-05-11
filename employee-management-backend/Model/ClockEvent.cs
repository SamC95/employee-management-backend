using System.ComponentModel.DataAnnotations;

namespace employee_management_backend.Model;

public class ClockEvent
{
    [Key] 
    public Guid EventId { get; set; } = Guid.NewGuid();
    
    public required string ClockId { get; set; } = "";

    public required string Type { get; set; } = "";
    
    public DateTime Timestamp
    {
        get => _timestamp.ToUniversalTime();
        set => _timestamp = value.ToUniversalTime();
    }
    
    private DateTime _timestamp;
}
