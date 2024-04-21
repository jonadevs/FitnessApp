using System.ComponentModel.DataAnnotations;
using FitnessApp.API.Entities;

namespace FitnessApp.API.Models;

public class CreateWorkoutDTO
{
    [Required(ErrorMessage = "You need to provide a name value.")]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public WorkoutType Type { get; set; }

    [Required(ErrorMessage = "You need to provide a start time value.")]
    public DateTime StartTime { get; set; }
}