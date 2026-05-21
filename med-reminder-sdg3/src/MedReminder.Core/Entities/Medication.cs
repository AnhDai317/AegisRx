namespace MedReminder.Core.Entities;

public class Medication
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
}
