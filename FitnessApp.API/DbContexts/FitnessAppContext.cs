using FitnessApp.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.API.DbContexts;

public class FitnessAppContext(DbContextOptions<FitnessAppContext> options) : DbContext(options)
{
    public DbSet<Workout> Workouts { get; set; }

    public DbSet<Set> Sets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Workout>().HasData(
            new Workout
            {
                Id = 1,
                Name = "Training 14/09",
                Type = WorkoutType.WeightTraining,
                StartTime = new DateTime(2023, 9, 14, 17, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2023, 9, 14, 18, 30, 0, DateTimeKind.Utc)
            },
            new Workout
            {
                Id = 2,
                Name = "Training 17/09",
                Type = WorkoutType.WeightTraining,
                StartTime = new DateTime(2023, 9, 17, 18, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2023, 9, 17, 19, 0, 0, DateTimeKind.Utc)
            },
            new Workout
            {
                Id = 3,
                Name = "Training 20/09",
                Type = WorkoutType.WeightTraining,
                StartTime = new DateTime(2023, 9, 20, 17, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2023, 9, 20, 18, 0, 0, DateTimeKind.Utc)
            },
            new Workout
            {
                Id = 4,
                Name = "Laufen 28/12",
                Type = WorkoutType.Running,
                StartTime = new DateTime(2023, 12, 28, 14, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2023, 12, 28, 14, 45, 0, DateTimeKind.Utc)
            },
            new Workout
            {
                Id = 5,
                Name = "Laufen 30/12",
                Type = WorkoutType.Running,
                StartTime = new DateTime(2023, 12, 30, 12, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2023, 12, 30, 12, 33, 0, DateTimeKind.Utc)
            });

        modelBuilder.Entity<Set>().HasData(
            new Set
            {
                Name = "Triceps pulldown",
                Intensity = "Easy",
                Id = 1,
                WorkoutId = 1
            },
            new Set
            {
                Name = "Shoulder press",
                Intensity = "Hard",
                Id = 2,
                WorkoutId = 1
            },
            new Set
            {
                Name = "Bicep curl",
                Intensity = "Easy",
                Id = 3,
                WorkoutId = 2
            },
            new Set
            {
                Name = "Chest fly",
                Intensity = "Medium",
                Id = 4,
                WorkoutId = 2
            },
            new Set
            {
                Name = "Hamstring curl",
                Intensity = "Easy",
                Id = 5,
                WorkoutId = 3
            });

        base.OnModelCreating(modelBuilder);
    }
}
