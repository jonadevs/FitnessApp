using System.ComponentModel.DataAnnotations;

namespace FitnessApp.API.Models
{
    /// <summary>
    /// A DTO to create a workout
    /// </summary>
    public class WorkoutForCreationDto
    {
        /// <summary>
        /// The name of the workout
        /// </summary>
        [Required(ErrorMessage = "You should provide a name value.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The type of the workout
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// The date of the workout
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// The length of the workout in minutes
        /// </summary>
        public int Length { get; set; }
    }
}