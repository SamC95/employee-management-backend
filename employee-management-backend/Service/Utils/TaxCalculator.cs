namespace employee_management_backend.Service.Utils;

public static class TaxCalculator
{
    // TODO - Only handles the most common UK tax codes for now, support for emergency tax codes and Scottish/Welsh tax codes to be added later
    public static decimal CalculateTax(decimal grossPay, string taxCode, int periodsPerYear = 12)
    {
        switch (taxCode)
        {
            case "1257L":
                const decimal personalAllowance = 12570.0m;
                const decimal basicRateUpper = 50270m;
                const decimal higherRateUpper = 125140m;

                var annualGrossPay = grossPay * periodsPerYear;

                var taxableIncome = Math.Max(0, annualGrossPay - personalAllowance);

                var basicRate = Math.Min(taxableIncome, basicRateUpper - personalAllowance) * 0.20m;
                
                var higherRate =
                    Math.Min(Math.Max(0, taxableIncome - (basicRateUpper - personalAllowance)),
                        higherRateUpper - basicRateUpper) * 0.40m;
                
                var additionalRate = Math.Max(0, taxableIncome - (higherRateUpper - personalAllowance)) * 0.45m;

                var annualTax = basicRate + higherRate + additionalRate;

                return annualTax / periodsPerYear;

            case "BR":
                return grossPay * 0.20m;

            case "D0":
                return grossPay * 0.40m;

            case "D1":
                return grossPay * 0.45m;

            case "NT":
                return 0;

            default:
                throw new ArgumentException($"Unsupported or unknown tax code: {taxCode}");
        }
    }
}