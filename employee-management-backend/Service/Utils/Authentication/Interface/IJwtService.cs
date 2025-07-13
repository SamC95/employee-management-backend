namespace employee_management_backend.Service.Utils.Authentication.Interface;

public interface IJwtService
{
    string GenerateJwtToken(string employeeId, string employeeName);
}