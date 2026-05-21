using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MedReminder.Core.Entities;

namespace MedReminder.Core.DTOs;

public class CreateMedicationDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty; // BrandName
    
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty; // GenericName + ActiveIngredient
    
    public DateOnly ExpiryDate { get; set; }
    
    public List<CreateReminderDto> Reminders { get; set; } = new();
}

public class CreateReminderDto
{
    [Required]
    public TimeSpan TimeOfDay { get; set; }
    
    [Required]
    public string TimezoneId { get; set; } = "UTC";
    
    [Required]
    public FrequencyType Frequency { get; set; }
}
