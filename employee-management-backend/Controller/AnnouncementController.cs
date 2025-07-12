using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace employee_management_backend.Controller;

[ApiController]
[Route("api/v1/announcement")]
public class AnnouncementController(IAnnouncementService announcementService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateAnnouncementPost([FromBody] Announcement announcement)
    {
        try
        {
            await announcementService.CreateAnnouncementPost(announcement);

            return Ok(new { message = $"Announcement successfully created and added to database." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"An unexpected error occured: {ex.Message}" });
        }
    }
}
