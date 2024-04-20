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

    public DateTime Date { get; set; }

    public int Length { get; set; }

    public ICollection<Set> Sets { get; set; } = new List<Set>();
}
