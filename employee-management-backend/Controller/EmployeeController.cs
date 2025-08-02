using System.Security.Claims;
using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace employee_management_backend.Controller;

[ApiController]
[Route("api/v1/employee")]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
    {
        try
        {
            var tempPassword = await employeeService.CreateEmployee(employee);

            return Ok(new
            {
                message = "Employee created successfully",
                temporaryPassword = tempPassword
            });
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

    [HttpPost("update/{employeeId}")]
    public async Task<IActionResult> UpdateEmployeeDetails(string employeeId, [FromBody] EmployeeUpdater patch)
    {
        if (employeeId != patch.EmployeeId)
        {
            return BadRequest(new { message = "The employee ID must match the ID in the body" });
        }
        
        var result = await employeeService.UpdateEmployeeDetails(patch);

        if (!result)
        {
            return NotFound(new { message = "The employee was not found" });
        }
        
        return Ok(new { message = "Employee updated successfully" });
    }

    [HttpPost("delete/{employeeId}")]
    public async Task<IActionResult> DeleteEmployee(string employeeId)
    {
        if (employeeId is null or "")
        {
            return BadRequest(new { message = "The employee ID is required" });
        }
        
        var deleted = await employeeService.DeleteEmployee(employeeId);

        if (!deleted)
        {
            return NotFound(new { message = "An employee was not found by this ID" });
        }
        
        return Ok(new { message = "Employee deleted successfully" });
    }

    [HttpGet("id/{employeeId}")]
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

    [HttpGet("{jobTitle}")]
    public async Task<IActionResult> GetEmployeesByJobTitle(string jobTitle)
    {
        try
        {
            var employees = await employeeService.GetEmployeesByJobTitle(jobTitle);

            return Ok(employees);
        }
        catch (Exception ex)
        {
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

    [Authorize]
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentlyLoggedInUserDetails()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        
        var employee = await employeeService.GetEmployeeById(userId);
        
        if (employee == null)
            return NotFound(new { message = "Employee was not found" });
        
        return Ok(employee);
    }
}
