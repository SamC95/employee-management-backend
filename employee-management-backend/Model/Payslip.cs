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
    
    public required double HoursWorked { get; set; }
    
    public required double HolidayHours { get; set; }
    
    public required bool HasPension { get; set; }
    
    public required bool HasUnion  { get; set; }
    
    public double TaxAmountPaid { get; set; }
    
    public double EmployeePensionAmountPaid { get; set; }
    
    public double EmployerPensionAmountPaid { get; set; }
    
    public required double PensionContributionPercentage { get; set; }
    
    public double EmployeeUnionAmountPaid { get; set; }
    
    public required double UnionContributionPercentage { get; set; }
    
    public double NationalInsuranceAmountPaid { get; set; }
    
    public required double PayPerHour { get; set; }
    
    public double GrossPay { get; set; }
    
    public double NetPay { get; set; }

    public List<DateOnly> SickDates { get; set; } = [];
}
