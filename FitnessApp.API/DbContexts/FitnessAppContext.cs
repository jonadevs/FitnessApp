using FitnessApp.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.API.DbContexts;

public class FitnessAppContext : DbContext
{
    public DbSet<Workout> Workouts { get; set; }

    public DbSet<Set> Sets { get; set; }

    public FitnessAppContext(DbContextOptions<FitnessAppContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Workout>().HasData(
            new Workout
            {
                Id = 1,
                Name = "Training 14/09",
                Type = WorkoutType.WeightTraining,
                Date = new DateTime(2023, 9, 14, 17, 0, 0, DateTimeKind.Utc),
                Length = 90
            },
            new Workout
            {
                Id = 2,
                Name = "Training 17/09",
                Type = WorkoutType.WeightTraining,
                Date = new DateTime(2023, 9, 17, 18, 0, 0, DateTimeKind.Utc),
                Length = 120
            },
            new Workout
            {
                Id = 3,
                Name = "Training 20/09",
                Type = WorkoutType.WeightTraining,
                Date = new DateTime(2023, 9, 20, 17, 0, 0, DateTimeKind.Utc),
                Length = 120
            },
            new Workout
            {
                Id = 4,
                Name = "Laufen 28/12",
                Type = WorkoutType.Running,
                Date = new DateTime(2023, 12, 28, 14, 0, 0, DateTimeKind.Utc),
                Length = 31
            },
            new Workout
            {
                Id = 5,
                Name = "Laufen 30/12",
                Type = WorkoutType.Running,
                Date = new DateTime(2023, 12, 30, 12, 0, 0, DateTimeKind.Utc),
                Length = 33
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
