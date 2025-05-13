using employee_management_backend.Model;
using employee_management_backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace employee_management_backend.Controller;

[ApiController]
[Route("api/employee")]
public class EmployeeController(EmployeeService employeeService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
    {
        try
        {
            await employeeService.CreateEmployee(employee);

            return Ok(new { message = "Employee created successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }
}
