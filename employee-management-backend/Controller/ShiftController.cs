using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace employee_management_backend.Controller;


[ApiController]
[Route("api/shift")]
public class ShiftController(IShiftService shiftService) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> AddWorkShift([FromBody] WorkShift shift)
    {
        try
        {
            await shiftService.AddWorkShift(shift);

            return Ok(new { message = $"Shift successfully added for {shift.EmployeeId}" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"An unexpected error occurred: {ex.Message}" });
        }
    }
}
