using FitnessApp.API.Entities;

namespace FitnessApp.Test
{
    public class WorkoutTests
    {
        [Fact]
        public void WorkoutConstructor_ConstructWorkout_NameMustBeTrainingHeute()
        {
            var workout = new Workout
            {
                Name = "Training heute",
            };

            Assert.Equal("Training heute", workout.Name);
        }

        [Fact]
        public void WorkoutConstructor_ConstructWorkout_LengthMustBe60()
        {
            var workout = new Workout
            {
                StartTime = new DateTime(2022, 1, 1, 12, 0, 0),
                EndTime = new DateTime(2022, 1, 1, 13, 0, 0),
            };

            Assert.Equal(60, workout.LengthInMinutes);
        }
    }
}
