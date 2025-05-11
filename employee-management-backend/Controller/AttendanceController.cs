using employee_management_backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace employee_management_backend.Controller;

[ApiController]
[Route("api/attendance")]
public class ClockEventController : ControllerBase
{
    private readonly ClockEventService _clockEventService;

    public ClockEventController(ClockEventService clockEventService)
    {
        _clockEventService = clockEventService;
    }
    
    [HttpGet("clock")]
    public async Task<IActionResult> 
}