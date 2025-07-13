using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using employee_management_backend.Service.Utils.Authentication.Interface;
using Microsoft.AspNetCore.Mvc;

namespace employee_management_backend.Controller;

[ApiController]
[Route("api/v1/login")]
public class LoginController(ILoginService loginService, IJwtService jwtService) : ControllerBase
{
    [HttpPost("validate")]
    public async Task<IActionResult> ValidateLogin([FromBody] LoginDetails loginDetails)
    {
        try
        {
            var employee = await loginService.ValidateLogin(loginDetails);

            if (employee == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var token = jwtService.GenerateJwtToken(employee.EmployeeId, $"{employee.FirstName} {employee.LastName}");

            return Ok(new { token });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new { message = $"An unexpected error occurred whilst validating your login: {ex.Message}" });
        }
    }
}