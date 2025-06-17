using System.ComponentModel.DataAnnotations;

namespace employee_management_backend.Model;

public class Payslip
{
    [Key] public Guid PayslipId { get; set; } = Guid.NewGuid();
    
    public required string? EmployeeId { get; set; }
    
    public required string EmployeeName { get; set; }
    
    public required string NationalInsuranceNumber { get; set; }
    
    public required string NationalInsuranceCategory { get; set; }
    
    public required DateOnly PayslipStartDate { get; set; }
    
    public required DateOnly PayslipEndDate { get; set; }
    
    public required string CompanyName { get; set; }
    
    public required string EmployeeDepartment { get; set; }
    
    public required int DaysWorkedPerWeek { get; set; }
    
    public required string TaxCode { get; set; }
    
    public required decimal HoursWorked { get; set; }
    
    public required decimal HolidayHours { get; set; }
    
    public required bool HasPension { get; set; }
    
    public required bool HasUnion  { get; set; }
    
    public decimal TaxAmountPaid { get; set; }
    
    public decimal EmployeePensionAmountPaid { get; set; }
    
    public decimal EmployerPensionAmountPaid { get; set; }
    
    public required decimal PensionContributionPercentage { get; set; }
    
    public decimal EmployeeUnionAmountPaid { get; set; }
    
    public required decimal UnionContributionPercentage { get; set; }
    
    public decimal NationalInsuranceAmountPaid { get; set; }
    
    public required decimal PayPerHour { get; set; }
    
    public decimal GrossPay { get; set; }
    
    public decimal NetPay { get; set; }

    public List<DateOnly> SickDates { get; set; } = [];
}
