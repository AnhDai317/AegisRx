using Microsoft.AspNetCore.Mvc;
using MedReminder.Core.Entities;

namespace MedReminder.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicationsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[] { "Medication 1", "Medication 2" });
    }
}
