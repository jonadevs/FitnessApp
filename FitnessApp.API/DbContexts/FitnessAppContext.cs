using FitnessApp.API.Entities;
using FitnessApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.API.DbContexts
{
    public class FitnessAppContext : DbContext
    {
        public DbSet<Workout> Workouts { get; set; }

        public DbSet<Set> Sets { get; set; }

        public FitnessAppContext(DbContextOptions<FitnessAppContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workout>().HasData(
                new Workout("Training 14/09")
                {
                    Id = 1
                },
                new Workout("Training 17/09")
                {
                    Id = 2
                },
                new Workout("Training 20/09")
                {
                    Id = 3
                });

            modelBuilder.Entity<Set>().HasData(
                new Set("Triceps pulldown", "Easy")
                {
                    Id = 1,
                    WorkoutId = 1
                },
                new Set("Shoulder press", "Hard")
                {
                    Id = 2,
                    WorkoutId = 1
                },
                new Set("Bicep curl", "Easy")
                {
                    Id = 3,
                    WorkoutId = 2
                },
                new Set("Chest fly", "Medium")
                {
                    Id = 4,
                    WorkoutId = 2

                },
                new Set("Hamstring curl", "Easy")
                {
                    Id = 5,
                    WorkoutId = 3
                });

            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("connectionstring");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
