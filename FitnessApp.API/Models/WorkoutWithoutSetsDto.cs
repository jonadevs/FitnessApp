namespace FitnessApp.API.Models
{
    /// <summary>
    /// A DTO for a workout without sets
    /// </summary>
    public class WorkoutWithoutSetsDto
    {
        /// <summary>
        /// The id of the workout
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the workout
        /// </summary>
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
