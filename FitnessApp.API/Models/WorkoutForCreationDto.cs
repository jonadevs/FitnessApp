using System.ComponentModel.DataAnnotations;

namespace FitnessApp.API.Models
{
    public class WorkoutForCreationDto
    {
        [Required(ErrorMessage = "You should provide a name value.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}