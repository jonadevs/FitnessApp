using FitnessApp.API.Entities;

namespace FitnessApp.API.Services
{
    public interface IFitnessAppRepository
    {
        Task<IEnumerable<Workout>> GetWorkoutsAsync();

        Task<Workout?> GetWorkoutAsync(int workoutId, bool includeSets);

        Task<bool> WorkoutExistsAsync(int workoutId);

        Task<IEnumerable<Set>> GetSetsForWorkoutAsync(int workoutId);

        Task<Set?> GetSetForWorkoutAsync(int workoutId, int setId);

        Task AddSetForWorkoutAsync(int workoutId, Set set);

        void DeleteSet(Set set);

        Task<bool> SaveChangesAsync();
    }
}
