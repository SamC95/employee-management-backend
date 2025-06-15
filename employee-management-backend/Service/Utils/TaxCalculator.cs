namespace employee_management_backend.Service.Utils;

public static class TaxCalculator
{
    // TODO - Only handles the most common UK tax codes for now, support for emergency tax codes and Scottish/Welsh tax codes to be added later
    public static double CalculateTax(double grossPay, string taxCode, int periodsPerYear = 12)
    {
        switch (taxCode)
        {
            case "1257L":
                const double personalAllowance = 12570.0;
                var annualGrossPay = grossPay * periodsPerYear;
                
                var taxableIncome = Math.Max(0, annualGrossPay - personalAllowance);

                var basicRate = Math.Min(taxableIncome, 50270 - personalAllowance) * 0.20;
                var higherRate =
                    Math.Min(Math.Max(0, taxableIncome - (50270 - personalAllowance)), 125140 - 50270) * 0.40;
                var additionalRate = Math.Max(0, taxableIncome - 125140 + personalAllowance) * 0.45;

                var annualTax = basicRate + higherRate + additionalRate;
                
                return annualTax / periodsPerYear;

            case "BR":
                return grossPay * 0.20;

            case "D0":
                return grossPay * 0.40;

            case "D1":
                return grossPay * 0.45;

            case "NT":
                return 0;

            default:
                throw new ArgumentException($"Unsupported or unknown tax code: {taxCode}");
        }
    }
}
