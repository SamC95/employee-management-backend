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
}
