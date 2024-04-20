using FitnessApp.API.DbContexts;
using FitnessApp.API.Entities;
using FitnessApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.API.Services;

public class WorkoutService : IWorkoutService
{
    private readonly FitnessAppContext _context;

    public WorkoutService(FitnessAppContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Workout>> GetWorkoutsAsync()
    {
        return await _context.Workouts.OrderBy(workout => workout.Date).ToListAsync();
    }

    public async Task<(IEnumerable<Workout>, PaginationMetadata)> GetWorkoutsAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
    {
        var workouts = _context.Workouts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            workouts = workouts.Where(workout => workout.Name == name.Trim());
        }
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            workouts = workouts.Where(workout => workout.Name.Contains(searchQuery.Trim()));
        }

        var totalItemCount = await workouts.CountAsync();
        var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

        workouts = workouts.OrderBy(workout => workout.Date)
                        .Skip(pageSize * (pageNumber - 1))
                        .Take(pageSize);

        return (await workouts.ToListAsync(), paginationMetadata);
    }

    public async Task<Workout?> GetWorkoutByIdAsync(int workoutId)
    {
        var workouts = await _context.Workouts.Where(workout => workout.Id == workoutId).FirstOrDefaultAsync();

        return workouts;
    }

    public async Task<Workout?> GetWorkoutByIdWithSetsAsync(int workoutId)
    {
        var workouts = await _context.Workouts.Include(workout => workout.Sets).Where(workout => workout.Id == workoutId).FirstOrDefaultAsync();

        return workouts;
    }

    public async Task<bool> WorkoutExistsAsync(int workoutId)
    {
        return await _context.Workouts.AnyAsync(workout => workout.Id == workoutId);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }

    public async Task<bool> WorkoutNameExistsAsync(string name)
    {
        return await _context.Workouts.AnyAsync(workout => workout.Name.Equals(name));
    }

    public void CreateWorkout(Workout newWorkout)
    {
        _context.Workouts.Add(newWorkout);
    }
}
