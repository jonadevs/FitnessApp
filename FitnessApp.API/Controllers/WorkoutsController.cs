using System.Text.Json;
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
        const int maxWorkoutsPageSize = 20;

        public WorkoutsController(IFitnessAppRepository fitnessAppRepository, IMapper mapper)
        {
            _fitnessAppRepository = fitnessAppRepository ??
                throw new ArgumentNullException(nameof(fitnessAppRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutWithoutSetsDto>>> GetWorkouts(string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if(pageSize > maxWorkoutsPageSize)
            {
                pageSize = maxWorkoutsPageSize;
            }

            var (workoutEntities, paginationMetadata) = await _fitnessAppRepository.GetWorkoutsAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

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
