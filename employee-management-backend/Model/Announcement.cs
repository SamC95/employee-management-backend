using System.ComponentModel.DataAnnotations;

namespace employee_management_backend.Model;

public class Announcement
{
    [Key]
    public required Guid EventId { get; init; } = Guid.NewGuid();
    
    [MaxLength(10)]
    public required string? CreatedByEmployeeId { get; set; }
    
    public required DateOnly CreationDate { get; init; } = DateOnly.FromDateTime(DateTime.Now);

    public DateOnly? LastModifiedDate { get; init; }
    
    [MaxLength(100)]
    public required string? Title { get; set; }
    
    [MaxLength(2000)]
    public required string? Description { get; set; }
    
    [MaxLength(20)]
    public required string? Audience { get; set; }

    public required int ReadCount { get; init; } = 0;
}
