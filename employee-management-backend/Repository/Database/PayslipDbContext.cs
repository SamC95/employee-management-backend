using employee_management_backend.Model;
using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Repository.Database;

public class PayslipDbContext(DbContextOptions<PayslipDbContext> options) : DbContext(options)
{
    public DbSet<Payslip> Payslips { get; set; }
}
