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
    }
}
