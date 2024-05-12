namespace FitnessApp.API.Models;

public class WorkoutWithoutSetsDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public DateTime? StartTime { get; set; }

    public int LengthInMinutes { get; set; }
}
