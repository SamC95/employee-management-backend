using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service.Interface;
using employee_management_backend.Service.Utils.Passwords;

namespace employee_management_backend.Service;

public class LoginService(IEmployeeRepository employeeRepository) : ILoginService
{
    public async Task<Employee?> ValidateLogin(LoginDetails loginDetails)
    {
        ArgumentNullException.ThrowIfNull(loginDetails);

        var employee = await employeeRepository.GetEmployeeById(loginDetails.UserId);

        if (employee == null || string.IsNullOrEmpty(loginDetails.Password))
            return null;

        var doesPasswordMatch = employee.Password != null &&
                                HashPasswordUtil.VerifyPasswordHash(loginDetails.Password, employee.Password);
        return doesPasswordMatch ? employee : null;
    }
}