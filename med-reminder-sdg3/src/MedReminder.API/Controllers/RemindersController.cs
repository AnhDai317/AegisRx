using Microsoft.AspNetCore.Mvc;

namespace MedReminder.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RemindersController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[] { "Reminder 1", "Reminder 2" });
    }
}
