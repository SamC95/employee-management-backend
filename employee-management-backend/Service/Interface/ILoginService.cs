using employee_management_backend.Model;

namespace employee_management_backend.Service.Interface;

public interface ILoginService
{
    Task<Employee?> ValidateLogin(LoginDetails loginDetails);
}
