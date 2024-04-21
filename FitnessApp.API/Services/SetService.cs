using FitnessApp.API.DbContexts;
using FitnessApp.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.API.Services;

public class SetService : ISetService
{
    private readonly FitnessAppContext _context;
    private readonly IWorkoutService _workoutService;

    public SetService(FitnessAppContext context, IWorkoutService workoutService)
    {
        _context = context;
        _workoutService = workoutService;
    }

    public async Task<IEnumerable<Set>> GetSetsForWorkoutAsync(int workoutId)
    {
        return await _context.Sets.Where(set => set.WorkoutId == workoutId).ToListAsync();
    }

    public async Task<Set?> GetSetForWorkoutAsync(int workoutId, int setId)
    {
        return await _context.Sets.Where(set => set.WorkoutId == workoutId && set.Id == setId).FirstOrDefaultAsync();
    }

    public async Task AddSetForWorkoutAsync(int workoutId, Set set)
    {
        var workout = await _workoutService.GetWorkoutByIdWithSetsAsync(workoutId);
        if (workout == null)
        {
            return;
        }

        workout.Sets.Add(set);
    }

    public void DeleteSet(Set set)
    {
        _context.Sets.Remove(set);
    }
}
