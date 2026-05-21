using MedReminder.Core.Entities;

namespace MedReminder.Core.Services;

public class MedicationService
{
    public bool IsMedicationExpired(Medication medication)
    {
        return medication.ExpiryDate < DateTime.UtcNow;
    }

    public bool HasScheduleConflict(Reminder newReminder, IEnumerable<Reminder> existingReminders)
    {
        // Basic check for exact same time
        return existingReminders.Any(r => r.TimeOfDay == newReminder.TimeOfDay);
    }
}
