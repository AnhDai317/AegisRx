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

        // Basic check for exact same time and frequency at the DB level
        return existingRemindersQuery.Any(r => 
            r.TimeOfDay == newReminder.TimeOfDay && 
            r.Frequency == newReminder.Frequency);
    }
}
