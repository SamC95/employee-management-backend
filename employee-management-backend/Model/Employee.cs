using System.ComponentModel.DataAnnotations;

namespace employee_management_backend.Model;

public class Employee
{
    [Key]
    public required string EmployeeId { get; set; }
    public required string ClockId { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNum { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string PostCode { get; set; }
    public required string Country { get; set; }
    public required string Gender { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required DateOnly DateHired { get; set; }
    public required bool IsAdmin { get; set; }
    public required bool IsManager { get; set; }
    public required bool IsActive { get; set; }
    public required string JobTitle { get; set; }
}
