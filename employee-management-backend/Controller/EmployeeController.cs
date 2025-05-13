using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace employee_management_backend.Controller;

[ApiController]
[Route("api/employee")]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase
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

    [HttpGet("{employeeId}")]
    public async Task<IActionResult> GetEmployeeById(string employeeId)
    {
        try
        {
            var employee = await employeeService.GetEmployeeById(employeeId);

            return employee == null ? Ok(new {}) : Ok(employee);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving employee data for id {employeeId}: {ex.Message}");
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }
}
