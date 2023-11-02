using FitnessApp.API.Models;

namespace FitnessApp.API
{
    public class WorkoutsDataStore
    {
        public List<WorkoutDto> Workouts { get; set; }

        // public static WorkoutsDataStore Current { get; } = new WorkoutsDataStore();

        public WorkoutsDataStore()
        {
            Workouts = new List<WorkoutDto>()
            {
                new WorkoutDto()
                {
                    Id = 1,
                    Name = "Training 14/09",
                    Sets = new List<SetDto>()
                    {
                        new SetDto()
                        {
                            Id = 1,
                            Name = "Triceps pulldown",
                            Intensity = "Easy"
                        },
                        new SetDto()
                        {
                            Id = 2,
                            Name = "Shoulder press",
                            Intensity = "Hard"
                        }
                    }
                },
                new WorkoutDto()
                {
                    Id = 2,
                    Name = "Training 17/09",
                    Sets = new List<SetDto>()
                    {
                        new SetDto()
                        {
                            Id = 1,
                            Name = "Bicep curl",
                            Intensity = "Easy"
                        },
                        new SetDto()
                        {
                            Id = 2,
                            Name = "Chest fly",
                            Intensity = "Medium"
                        }
                    }
                },
                new WorkoutDto()
                {
                    Id = 3,
                    Name = "Training 20/09",
                    Sets = new List<SetDto>()
                    {
                        new SetDto()
                        {
                            Id = 1,
                            Name = "Hamstring curl",
                            Intensity = "Easy"
                        }
                    }
                },
            };
        }
    }
}
