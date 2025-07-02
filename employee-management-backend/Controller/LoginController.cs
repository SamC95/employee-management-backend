using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace employee_management_backend.Controller;

[ApiController]
[Route("api/v1/login")]
public class LoginController(ILoginService loginService) : ControllerBase
{
    [HttpPost("validate")]
    public async Task<IActionResult> ValidateLogin([FromBody] LoginDetails loginDetails)
    {
        try
        {
            var userValid = await loginService.ValidateLogin(loginDetails);
            
            return Ok(userValid);
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
