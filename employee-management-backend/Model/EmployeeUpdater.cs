using System.ComponentModel.DataAnnotations;

namespace employee_management_backend.Model;

public class EmployeeUpdater
{
    [Key]
    public required string EmployeeId { get; set; }
    
    public string? ClockId { get; set; }
    public string? FirstName { get; set; }
    
    public string? MiddleName { get; set; }
    
    public string? LastName { get; set; }
    
    public string? Email { get; set; }
    
    public string? PhoneNum { get; set; }
    
    public string? Address { get; set; }
    
    public string? City { get; set; }
    
    public string? PostCode { get; set; }
    
    public string? Country { get; set; }
    
    public string? Gender { get; set; }
    
    public DateOnly? DateOfBirth { get; set; }
    
    public DateOnly? DateHired { get; set; }
    
    public bool? IsAdmin { get; set; }
    
    public bool? IsManager { get; set; }
    
    public bool? IsActive { get; set; }
    
    public string? JobTitle { get; set; }
    
    public decimal? PayPerHour { get; set; }
    
    public string? Password { get; set; }
    
    public bool? HasSetOwnPassword { get; set; }

    public string? NationalInsuranceNumber { get; set; }
    
    public string? NationalInsuranceCategory { get; set; }
    
    public string? TaxCode { get; set; }
    
    public bool? HasPension { get; set; }
    
    public decimal? EmployeePensionContributionPercentage { get; set; }
    
    public decimal? EmployerPensionContributionPercentage { get; set; }
    
    public bool? HasUnion { get; set; }
    
    public decimal? UnionContributionPercentage { get; set; }
}
