using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.API.Entities;

public class Set
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MaxLength(200)]
    public string Intensity { get; set; }

    [ForeignKey("WorkoutId")]
    public Workout? Workout { get; set; }

    public int WorkoutId { get; set; }

    public Set(string name, string intensity)
    {
        Name = name;
        Intensity = intensity;
    }
}
