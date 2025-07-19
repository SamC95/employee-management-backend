using System.Security.Claims;
using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentAnnouncements([FromQuery] int limit)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var announcements = await announcementService.GetRecentAnnouncements(userId, limit);

            return Ok(announcements);
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
