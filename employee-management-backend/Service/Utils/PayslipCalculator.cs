using employee_management_backend.Model;

namespace employee_management_backend.Service.Utils;

public class PayslipCalculator()
{
    public static void Calculate(Payslip payslip)
    {
        payslip.GrossPay = (payslip.HoursWorked + payslip.HolidayHours) * payslip.PayPerHour;

        payslip.EmployeePensionAmountPaid =
            payslip.HasPension ? payslip.GrossPay * (payslip.PensionContributionPercentage / 100.0m) : 0m;

        payslip.EmployerPensionAmountPaid =
            payslip.HasPension ? payslip.GrossPay * (payslip.PensionContributionPercentage / 100.0m) : 0m;

        payslip.EmployeeUnionAmountPaid =
            payslip.HasUnion ? payslip.GrossPay * (payslip.UnionContributionPercentage / 100.0m) : 0m;

        payslip.TaxAmountPaid = TaxCalculator.CalculateTax(payslip.GrossPay, payslip.TaxCode);

        var weeksInPeriod = CalculateWeeksInPayPeriod(payslip.PayslipStartDate, payslip.PayslipEndDate);
        var sickDaysByWeek = GetSickDaysByWeek(payslip.SickDates, payslip.PayslipStartDate, weeksInPeriod);

        payslip.NationalInsuranceAmountPaid = NationalInsuranceCalculator.CalculateNationalInsurance(payslip.GrossPay,
            weeksInPeriod, sickDaysByWeek, payslip.NationalInsuranceCategory, payslip.DaysWorkedPerWeek);

        payslip.NetPay = payslip.GrossPay - (payslip.TaxAmountPaid + payslip.EmployeePensionAmountPaid +
                                             payslip.NationalInsuranceAmountPaid + payslip.EmployeeUnionAmountPaid);
    }

    private static Dictionary<int, int> GetSickDaysByWeek(List<DateOnly> sickDates, DateOnly startDate,
        double weeksInPayPeriod)
    {
        var result = new Dictionary<int, int>();

        foreach (var weekIndex in sickDates.Select(date => date.DayNumber - startDate.DayNumber)
                     .Select(daysFromStart => daysFromStart / 7)
                     .Where(weekIndex => weekIndex >= 0 && weekIndex < weeksInPayPeriod))
        {
            result[weekIndex] = result.GetValueOrDefault(weekIndex, 0) + 1;
        }

        return result;
    }

    private static double CalculateWeeksInPayPeriod(DateOnly startDate, DateOnly endDate)
    {
        if (endDate < startDate) 
            throw new ArgumentException("end date cannot be before the start date");
        
        var totalDays = endDate.DayNumber - startDate.DayNumber + 1;
        var weeksInPeriod = Math.Ceiling(totalDays / 7.0);
        
        return weeksInPeriod;
    }
}
