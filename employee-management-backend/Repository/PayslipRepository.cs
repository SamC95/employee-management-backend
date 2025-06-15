using employee_management_backend.Model;
using employee_management_backend.Repository.Database;
using employee_management_backend.Repository.Interface;


namespace employee_management_backend.Repository;

public class PayslipRepository(PayslipDbContext context) : IPayslipRepository
{
    public async Task CreatePayslip(Payslip payslip)
    {
        try
        {
            await context.Payslips.AddAsync(payslip);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred adding payslip to database: {ex.Message}");
        }
    }
}
