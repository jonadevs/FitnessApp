﻿namespace FitnessApp.API.Models;

public class WorkoutDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public DateTime? Date { get; set; }

    public int Length { get; set; }

    public int NumberOfSets
    {
        get
        {
            return Sets.Count;
        }
    }

    public ICollection<SetDto> Sets { get; set; } = new List<SetDto>();
}