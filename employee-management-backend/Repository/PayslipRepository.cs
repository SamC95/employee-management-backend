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
            throw new InvalidOperationException($"Failed to create payslip in the database: {ex.Message}");
        }
    }
}
