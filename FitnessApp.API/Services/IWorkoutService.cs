using FitnessApp.API.Entities;
using FitnessApp.API.Models;

namespace FitnessApp.API.Services;
public interface IWorkoutService
{
    void CreateWorkout(Workout newWorkout);

    Task<Workout?> GetWorkoutByIdAsync(int workoutId);

    Task<Workout?> GetWorkoutByIdWithSetsAsync(int workoutId);

    Task<IEnumerable<Workout>> GetWorkoutsAsync();

    Task<(IEnumerable<Workout>, PaginationMetadata)> GetWorkoutsAsync(string? name, string? searchQuery, int pageNumber, int pageSize);

    Task<bool> SaveChangesAsync();

    Task<bool> WorkoutExistsAsync(int workoutId);

    Task<bool> WorkoutNameExistsAsync(string name);
}