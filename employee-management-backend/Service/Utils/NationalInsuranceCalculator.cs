namespace employee_management_backend.Service.Utils;

public static class NationalInsuranceCalculator
{
    // Constants for 2025/26 weekly thresholds
    private const double PrimaryThreshold = 242.00;
    private const double UpperEarningsLimit = 967.00;

    // Calculates total National Insurance across multiple weeks accounting for different weekly pay and unpaid sick days.
    public static double CalculateNationalInsurance(double grossPay, double weeksInPeriod,
        Dictionary<int, int> sickDaysByWeek, string category, int daysWorkedPerWeek)
    {
        if (daysWorkedPerWeek is <= 0 or > 7)
            throw new ArgumentException("Days worked per week must be between 1 and 7");
        
        category = category.ToUpperInvariant();
        var weeklyGross = grossPay / weeksInPeriod;
        double totalNationalInsurance = 0;

        for (var week = 0; week < weeksInPeriod; week++)
        {
            var sickDays = sickDaysByWeek.GetValueOrDefault(week, 0);
            var adjustedPay = AdjustWeeklyPayForSickDays(weeklyGross, sickDays, daysWorkedPerWeek);

            totalNationalInsurance += category switch
            {
                "A" or "H" or "M" => CalculateStandardNationalInsurance(adjustedPay),
                "J" or "Z"        => CalculateDeferredNationalInsurance(adjustedPay),
                "B" or "C"        => 0,
                _ => throw new ArgumentException($"Unsupported NI category: {category}")
            };
        }

        return Math.Round(totalNationalInsurance, 2);
    }

    private static double AdjustWeeklyPayForSickDays(double weeklyPay, int sickDays, int daysWorkedPerWeek)
    {
        var daysWorked = Math.Max(0, daysWorkedPerWeek - sickDays);
        return weeklyPay * daysWorked / daysWorkedPerWeek;
    }

    private static double CalculateStandardNationalInsurance(double weeklyPay)
    {
        if (weeklyPay <= PrimaryThreshold)
            return 0;

        var basicBand = Math.Min(weeklyPay, UpperEarningsLimit) - PrimaryThreshold;
        var upperBand = Math.Max(0, weeklyPay - UpperEarningsLimit);

        return Math.Round(basicBand * 0.08 + upperBand * 0.02, 2);
    }

    private static double CalculateDeferredNationalInsurance(double weeklyPay)
    {
        if (weeklyPay <= UpperEarningsLimit)
            return 0;

        var upperBand = weeklyPay - UpperEarningsLimit;
        return Math.Round(upperBand * 0.02, 2);
    }
}
