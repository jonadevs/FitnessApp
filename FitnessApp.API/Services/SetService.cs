using FitnessApp.API.DbContexts;
using FitnessApp.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.API.Services;

public class SetService(FitnessAppContext context, IWorkoutService workoutService) : ISetService
{
    private readonly FitnessAppContext _context = context;
    private readonly IWorkoutService _workoutService = workoutService;

    public async Task<IEnumerable<Set>> GetSetsForWorkoutAsync(int workoutId)
    {
        return await _context.Sets.Where(x => x.WorkoutId == workoutId).ToListAsync();
    }

    public async Task<Set?> GetSetForWorkoutAsync(int workoutId, int setId)
    {
        return await _context.Sets.Where(x => x.WorkoutId == workoutId && x.Id == setId).FirstOrDefaultAsync();
    }

    public async Task AddSetForWorkoutAsync(int workoutId, Set set)
    {
        var workout = await _workoutService.GetWorkoutByIdWithSetsAsync(workoutId);
        if (workout is null)
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
