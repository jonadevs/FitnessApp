using AutoMapper;
using FitnessApp.API.Models;
using FitnessApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers
{
    [ApiController]
    [Route("api/workouts")]
    public class WorkoutsController : ControllerBase
    {
        private readonly IFitnessAppRepository _fitnessAppRepository;
        private readonly IMapper _mapper;

        public WorkoutsController(IFitnessAppRepository fitnessAppRepository, IMapper mapper)
        {
            _fitnessAppRepository = fitnessAppRepository ??
                throw new ArgumentNullException(nameof(fitnessAppRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutWithoutSetsDto>>> GetWorkouts()
        {
            var workoutEntities = await _fitnessAppRepository.GetWorkoutsAsync();

            return Ok(_mapper.Map<IEnumerable<WorkoutWithoutSetsDto>>(workoutEntities));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkout(int id, bool includeSets = false)
        {
            var workout = await _fitnessAppRepository.GetWorkoutAsync(id, includeSets);

            if (workout == null)
            {
                return NotFound();
            }

            if (includeSets)
            {
                return Ok(_mapper.Map<WorkoutDto>(workout));
            }

            return Ok(_mapper.Map<WorkoutWithoutSetsDto>(workout));
        }
    }
}
