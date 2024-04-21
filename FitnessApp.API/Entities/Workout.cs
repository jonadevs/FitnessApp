using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.API.Entities;

public class Workout
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public WorkoutType Type { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int LengthInMinutes
    {
        get
        {
            return (int)(EndTime - StartTime).TotalMinutes;
        }
    }

    public ICollection<Set> Sets { get; set; } = new List<Set>();
}
