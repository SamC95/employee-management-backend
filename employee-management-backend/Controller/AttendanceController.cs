using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace employee_management_backend.Controller;

[ApiController]
[Route("api/attendance")]
public class AttendanceController(IAttendanceService attendanceService) : ControllerBase
{
    [HttpPost("clock")]
    public async Task<IActionResult> PostClockEvent([FromBody] ClockEvent clockEvent)
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
    public async Task<IActionResult> GetClockEventsByClockId(string clockId)
    {
        try
        {
            var clockEvents = await attendanceService.GetClockEventsByClockId(clockId);

            var result = clockEvents.Select(retrievedEvent => new
                {
                    retrievedEvent.ClockId,
                    retrievedEvent.Type,
                    Timestamp = retrievedEvent.Timestamp.ToLocalTime()
                })
                .ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving clock events for id {clockId}: {ex.Message}");
            return StatusCode(500, new { message = $"Error occurred: {ex.Message}" });
        }
    }
}
