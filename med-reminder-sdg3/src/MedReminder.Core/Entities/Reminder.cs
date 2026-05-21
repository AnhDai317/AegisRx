namespace MedReminder.Core.Entities;

public class Reminder
{
    public int Id { get; set; }
    public int MedicationId { get; set; }
    public TimeSpan TimeOfDay { get; set; }
    public string Frequency { get; set; } = string.Empty; // e.g. "Daily", "EveryOtherDay"
    public Medication? Medication { get; set; }
}
