using System.ComponentModel.DataAnnotations;

namespace employee_management_backend.Model;

public class Payslip
{
    [Key] public Guid PayslipId { get; set; } = Guid.NewGuid();
    
    [MaxLength(10)]
    public required string? EmployeeId { get; set; }
    
    [MaxLength(100)]
    public string? EmployeeName { get; set; }
    
    public required DateOnly PayslipStartDate { get; set; }
    
    public required DateOnly PayslipEndDate { get; set; }
    
    [MaxLength(100)]
    public required string CompanyName { get; set; }
    
    [MaxLength(100)]
    public required string EmployeeDepartment { get; set; }
    
    public required int DaysWorkedPerWeek { get; set; }
    
    public required decimal HoursWorked { get; set; }
    
    public required decimal HolidayHours { get; set; }
    
    public decimal TaxAmountPaid { get; set; }
    
    public decimal EmployeePensionAmountPaid { get; set; }
    
    public decimal EmployerPensionAmountPaid { get; set; }
    
    public decimal EmployeeUnionAmountPaid { get; set; }
    
    public decimal NationalInsuranceAmountPaid { get; set; }
    
    public decimal GrossPay { get; set; }
    
    public decimal NetPay { get; set; }

    public List<DateOnly> SickDates { get; set; } = [];
}
