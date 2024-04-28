using FitnessApp.API.Entities;

namespace FitnessApp.API.Services;
public interface ISetService
{
    Task AddSetForWorkoutAsync(int workoutId, Set set);

    void DeleteSet(Set set);

    Task<Set?> GetSetForWorkoutAsync(int workoutId, int setId);

    Task<IEnumerable<Set>> GetSetsForWorkoutAsync(int workoutId);
}