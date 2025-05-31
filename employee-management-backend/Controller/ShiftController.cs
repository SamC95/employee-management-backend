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

    [HttpPost("update/{shiftId}")]
    public async Task<IActionResult> AmendWorkShift(Guid shiftId, [FromBody] WorkShift shift)
    {
        if (shiftId != shift.ShiftId)
        {
            return BadRequest(new { message = "The shift ID must match the ID in the body"});
        }

        var result = await shiftService.UpdateWorkShift(shift);

        if (!result)
        {
            return NotFound(new { message = "The shift was not found" });
        }
        
        return Ok(new { message = "Shift updated successfully" });
    }

    [HttpPost("delete/{shiftId}")]
    public async Task<IActionResult> DeleteWorkShift(string shiftId)
    {
        if (!Guid.TryParse(shiftId, out var parsedShiftId))
        {
            return BadRequest(new { message = "Invalid shift ID format." });
        }
        
        var deleted = await shiftService.DeleteWorkShift(parsedShiftId);

        if (!deleted)
        {
            return NotFound(new { message = "No shift found by this shift ID" });
        }
        
        return Ok(new { message = "Shift deleted successfully" });
    }

    [HttpPost("id/{shiftId:guid}")]
    public async Task<IActionResult> GetShiftById(Guid shiftId)
    {
        try
        {
            var shift = await shiftService.GetShiftById(shiftId);

            return shift == null ? NotFound(new { message = "Shift not found" }) : Ok(shift);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving shift data for id: {shiftId}: {ex.Message}");
            return StatusCode(500, new { message = $"An unexpected error occurred: {ex.Message}" });
        }
    }
}
