using employee_management_backend.Model;

namespace employee_management_backend.Repository.Interface;

public interface IPayslipRepository
{
    Task CreatePayslip(Payslip payslip);
}
