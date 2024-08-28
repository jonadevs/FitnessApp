using FitnessApp.API.DbContexts;
using FitnessApp.API.Entities;
using FitnessApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.API.Services;

public class WorkoutService(FitnessAppContext context) : IWorkoutService
{
    private readonly FitnessAppContext _context = context;

    public async Task<IEnumerable<Workout>> GetWorkoutsAsync()
    {
        return await _context.Workouts.OrderBy(workout => workout.StartTime).ToListAsync();
    }

    public async Task<(IEnumerable<Workout>, PaginationMetadata)> GetWorkoutsAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
    {
        var workouts = _context.Workouts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            workouts = workouts.Where(x => x.Name == name.Trim());
        }
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            workouts = workouts.Where(x => x.Name.Contains(searchQuery.Trim()));
        }

        var totalItemCount = await workouts.CountAsync();
        var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

        workouts = workouts.OrderBy(x => x.StartTime)
                        .Skip(pageSize * (pageNumber - 1))
                        .Take(pageSize);

        return (await workouts.ToListAsync(), paginationMetadata);
    }

    public async Task<Workout?> GetWorkoutByIdAsync(int workoutId)
    {
        return await _context.Workouts.Where(x => x.Id == workoutId).FirstOrDefaultAsync();
    }

    public async Task<Workout?> GetWorkoutByIdWithSetsAsync(int workoutId)
    {
        return await _context.Workouts.Include(x => x.Sets).Where(x => x.Id == workoutId).FirstOrDefaultAsync();
    }

    public async Task<bool> WorkoutExistsAsync(int workoutId)
    {
        return await _context.Workouts.AnyAsync(x => x.Id == workoutId);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }

    public async Task<bool> WorkoutNameExistsAsync(string name)
    {
        return await _context.Workouts.AnyAsync(x => x.Name.Equals(name));
    }

    public void CreateWorkout(Workout newWorkout)
    {
        _context.Workouts.Add(newWorkout);
    }
}
