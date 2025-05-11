using employee_management_backend.Model;
using employee_management_backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace employee_management_backend.Controller;

[ApiController]
[Route("api/attendance")]
public class AttendanceController(AttendanceService attendanceService) : ControllerBase
{
    [HttpPost("clock")]
    public async Task <IActionResult> PostClockEvent([FromBody] ClockEvent clockEvent)
    {
        if (clockEvent.ClockId == "")
        {
            return BadRequest("Invalid clock event data");
        }

        try
        {
            Console.WriteLine($"Clock ID: {clockEvent.ClockId}, Type: {clockEvent.Type}, Timestamp: {clockEvent.Timestamp}, EventId: {clockEvent.EventId}");
            await attendanceService.PostClockEvent(clockEvent);

            return Ok(new { message = "Clock event saved successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error occurred: {ex.Message}" });
        }
    }

    [HttpGet("clock/{clockId}")]
    public async Task<IActionResult> GetClockEvent(string clockId)
    {
        try
        {
            var clockEvents = await attendanceService.GetClockEventsByClockId(clockId);
            
            var result = clockEvents.Select(retrievedEvent => new
            {
                retrievedEvent.ClockId,
                retrievedEvent.Type,
                retrievedEvent.Timestamp
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving clock events for id {clockId}: {ex.Message}");
            return StatusCode(500, new { message = $"Error occurred: {ex.Message}" });
        }
    }
}
