using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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
        catch (DbUpdateException dbEx) when (dbEx.InnerException is PostgresException { SqlState: "23505" })
        {
            return Conflict(new { message = "An employee with this Employee ID or Clock ID already exists." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"An unexpected error occurred: {ex.Message}" });
        }
    }

    [HttpGet("{employeeId}")]
    public async Task<IActionResult> GetEmployeeById(string employeeId)
    {
        try
        {
            var employee = await employeeService.GetEmployeeById(employeeId);

            return employee == null ? Ok(new { }) : Ok(employee);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving employee data for id {employeeId}: {ex.Message}");
            return StatusCode(500, new { message = $"An unexpected error occurred: {ex.Message}" });
        }
    }

    [HttpGet("clock/{clockId}")]
    public async Task<IActionResult> CheckClockIdExists(string clockId)
    {
        try
        {
            var clockIdExists = await employeeService.CheckClockIdExists(clockId);

            return clockIdExists == null ? Ok(new { }) : Ok(clockIdExists);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving clock data for id {clockId}: {ex.Message}");
            return StatusCode(500, new { message = $"An unexpected error occurred: {ex.Message}" });
        }
    }
}
