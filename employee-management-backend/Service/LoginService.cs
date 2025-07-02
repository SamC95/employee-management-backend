using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service.Interface;
using employee_management_backend.Service.Utils.Passwords;

namespace employee_management_backend.Service;

public class LoginService(IEmployeeRepository employeeRepository) : ILoginService
{
    public async Task<bool> ValidateLogin(LoginDetails loginDetails)
    {
        ArgumentNullException.ThrowIfNull(loginDetails);

        var employee = await employeeRepository.GetEmployeeById(loginDetails.UserId);

        if (employee == null)
            return false;

        if (string.IsNullOrEmpty(employee.Password)) 
            return false;
        
        var doesPasswordMatch = HashPasswordUtil.VerifyPasswordHash(loginDetails.Password, employee.Password);
        return doesPasswordMatch;
    }
}
