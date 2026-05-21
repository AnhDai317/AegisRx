using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MedReminder.Core.Entities;
using MedReminder.Infrastructure.ExternalAPIs;
using MedReminder.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MedReminder.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicationsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly FdaClient _fdaClient;

    public MedicationsController(ApplicationDbContext dbContext, FdaClient fdaClient)
    {
        _dbContext = dbContext;
        _fdaClient = fdaClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var medications = await _dbContext.Medications.ToListAsync();
        return Ok(medications);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("Search query cannot be empty.");
        }

        var results = await _fdaClient.SearchDrugsAsync(query);
        return Ok(results);
    }
}
