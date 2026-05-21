using System;
using System.Linq;
using MedReminder.Core.Entities;

namespace MedReminder.Core.Services;

public class MedicationService
{
    /// <summary>
    /// Checks if a medication is expired based on the provided current date.
    /// In production, currentDate should be injected via an IClock service matching the user's timezone.
    /// </summary>
    public bool IsMedicationExpired(Medication medication, DateOnly currentDate)
    {
        if (medication == null) throw new ArgumentNullException(nameof(medication));
        return medication.ExpiryDate < currentDate;
    }

    /// <summary>
    /// Checks for a schedule conflict using an IQueryable to ensure evaluation happens at the database level.
    /// </summary>
    public bool HasScheduleConflict(Reminder newReminder, IQueryable<Reminder> existingRemindersQuery)
    {
        if (newReminder == null) throw new ArgumentNullException(nameof(newReminder));
        if (existingRemindersQuery == null) throw new ArgumentNullException(nameof(existingRemindersQuery));

        // Simplified robust check: Block if TimeOfDay overlaps regardless of Frequency to prevent accidental multi-dosing.
        // Also considers TimezoneId for cross-timezone overlap accuracy if implemented properly in the DB (here we enforce identical timezone check for basic conflict).
        return existingRemindersQuery.Any(r => 
            r.TimeOfDay == newReminder.TimeOfDay &&
            r.TimezoneId == newReminder.TimezoneId);
    }
}
