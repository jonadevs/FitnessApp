using FitnessApp.API.Entities;

namespace FitnessApp.API.Services
{
    public interface IFitnessAppRepository
    {
        Task<IEnumerable<Workout>> GetWorkoutsAsync();

        Task<(IEnumerable<Workout>, PaginationMetadata)> GetWorkoutsAsync(string? name, string? searchQuery, int pageNumber, int pageSize);

        Task<Workout?> GetWorkoutAsync(int workoutId, bool includeSets);

        Task<bool> WorkoutExistsAsync(int workoutId);
        
        Task<bool> WorkoutNameExistsAsync(string name);

        void CreateWorkout(Workout newWorkout);

        Task<IEnumerable<Set>> GetSetsForWorkoutAsync(int workoutId);

        Task<Set?> GetSetForWorkoutAsync(int workoutId, int setId);

        Task AddSetForWorkoutAsync(int workoutId, Set set);

        void DeleteSet(Set set);

        Task<bool> SaveChangesAsync();
    }
}
