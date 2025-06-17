using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace employee_management_backend.Controller;

[ApiController]
[Route("api/v1/holiday")]
public class HolidayController(IHolidayService holidayService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateHolidayRequest([FromBody] HolidayEvent holidayRequest)
    {
        if (holidayRequest.Status is "" or null)
        {
            return BadRequest(new { message = "Request is missing the status field" });
        }

        try
        {
            await holidayService.CreateHolidayRequest(holidayRequest);

            return Ok(new { message = $"Holiday request created successfully for {holidayRequest.EmployeeId}" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"An unexpected error occurred. {ex.Message}" });
        }
    }

    [HttpPost("update/status/{eventId:guid}")]
    public async Task<IActionResult> UpdateHolidayStatus(Guid eventId, [FromBody] HolidayEvent holidayEvent)
    {
        if (eventId != holidayEvent.EventId)
        {
            return BadRequest(new { message = "The event ID must match the ID in the body" });
        }

        var result = await holidayService.UpdateHolidayStatus(holidayEvent);

        if (!result)
        {
            return NotFound(new { message = "Holiday request by this event id could not be found." });
        }

        return Ok(new { message = "Holiday status updated successfully" });
    }
}
