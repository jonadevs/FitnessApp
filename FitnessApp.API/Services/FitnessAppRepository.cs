using FitnessApp.API.DbContexts;
using FitnessApp.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.API.Services
{
    public class FitnessAppRepository : IFitnessAppRepository
    {
        private readonly FitnessAppContext _context;

        public FitnessAppRepository(FitnessAppContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Workout>> GetWorkoutsAsync()
        {
            return await _context.Workouts.OrderBy(workout => workout.Name).ToListAsync();
        }

        public async Task<Workout?> GetWorkoutAsync(int workoutId, bool includeSets)
        {
            if (includeSets)
            {
                return await _context.Workouts.Include(workout => workout.Sets)
                    .Where(workout => workout.Id == workoutId).FirstOrDefaultAsync();
            }

            return await _context.Workouts
                .Where(workout => workout.Id == workoutId).FirstOrDefaultAsync();
        }

        public async Task<bool> WorkoutExistsAsync(int workoutId)
        {
            return await _context.Workouts.AnyAsync(workout => workout.Id == workoutId);
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
            var workout = await GetWorkoutAsync(workoutId, false);
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

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
