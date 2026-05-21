using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MedReminder.Core.Entities;
using MedReminder.Core.DTOs;
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMedicationDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var medication = new Medication
        {
            Name = request.Name,
            Description = request.Description,
            ExpiryDate = request.ExpiryDate
        };

        _dbContext.Medications.Add(medication);

        foreach (var reqReminder in request.Reminders)
        {
            var reminder = new Reminder
            {
                Medication = medication,
                TimeOfDay = reqReminder.TimeOfDay,
                TimezoneId = reqReminder.TimezoneId,
                Frequency = reqReminder.Frequency
            };
            _dbContext.Reminders.Add(reminder);
        }

        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = medication.Id }, medication);
    }
}
