namespace FitnessApp.API.Models;

public class SetDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Intensity { get; set; }
}
