using System.ComponentModel.DataAnnotations;

namespace employee_management_backend.Model;

public class Announcement
{
    [Key]
    public Guid EventId { get; init; } = Guid.NewGuid();
    
    [MaxLength(10)]
    public required string? CreatedByEmployeeId { get; set; }
    
    public DateTime CreationDate { get; init; } = DateTime.UtcNow;

    public DateTime? LastModifiedDate { get; init; }
    
    [MaxLength(100)]
    public required string? Title { get; set; }
    
    [MaxLength(2000)]
    public required string? Description { get; set; }
    
    [MaxLength(20)]
    public required string? Audience { get; set; }

    public int ReadCount { get; init; }
}
