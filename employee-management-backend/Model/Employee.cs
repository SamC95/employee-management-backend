using System.ComponentModel.DataAnnotations;
using employee_management_backend.Service.Utils.Passwords;

namespace employee_management_backend.Model;

public class Employee
{
    [MaxLength(10)]
    [Key]
    public required string EmployeeId { get; set; }
    
    [MaxLength(8)]
    public required string ClockId { get; set; }
    
    [MaxLength(25)]
    public required string FirstName { get; set; }
    
    [MaxLength(25)]
    public string? MiddleName { get; set; }
    
    [MaxLength(25)]
    public required string LastName { get; set; }
    
    [MaxLength(50)]
    public required string Email { get; set; }
    
    [MaxLength(15)]
    public required string PhoneNum { get; set; }
    
    [MaxLength(100)]
    public required string Address { get; set; }
    
    [MaxLength(50)]
    public required string City { get; set; }
    
    [MaxLength(10)]
    public required string PostCode { get; set; }
    
    [MaxLength(50)]
    public required string Country { get; set; }
    
    [MaxLength(15)]
    public required string Gender { get; set; }
    
    public required DateOnly DateOfBirth { get; set; }
    
    public required DateOnly DateHired { get; set; }
    
    public required bool IsAdmin { get; set; }
    
    public required bool IsManager { get; set; }
    
    public required bool IsActive { get; set; }
    
    [MaxLength(50)]
    public required string JobTitle { get; set; }
    
    public decimal PayPerHour { get; set; }
    
    [MaxLength(45)]
    public string? Password { get; set; } = CreateRandomPassword.GenerateRandomPassword(10);

    [MaxLength(9)]
    public required string NationalInsuranceNumber { get; set; }
    
    [MaxLength(5)]
    public required string NationalInsuranceCategory { get; set; }
    
    [MaxLength(10)]
    public required string TaxCode { get; set; }
    
    public required bool HasPension { get; set; }
    
    public required decimal EmployeePensionContributionPercentage { get; set; }
    
    public required decimal EmployerPensionContributionPercentage { get; set; }
    
    public required bool HasUnion { get; set; }
    
    public required decimal UnionContributionPercentage { get; set; }
}
