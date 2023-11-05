using System.Security.Claims;
using System.Text.Json;
using AutoMapper;
using FitnessApp.API.Entities;
using FitnessApp.API.Filters;
using FitnessApp.API.Models;
using FitnessApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/workouts")]
    public class WorkoutsController : ControllerBase
    {
        private readonly IFitnessAppRepository _fitnessAppRepository;
        private readonly IMapper _mapper;
        const int maxWorkoutsPageSize = 20;

        public WorkoutsController(IFitnessAppRepository fitnessAppRepository, IMapper mapper)
        {
            _fitnessAppRepository = fitnessAppRepository ?? throw new ArgumentNullException(nameof(fitnessAppRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get a list of workouts
        /// </summary>
        /// <param name="name">Filter by name</param>
        /// <param name="searchQuery">Search by name</param>
        /// <param name="pageNumber">Page number to return</param>
        /// <param name="pageSize">Page size to return</param>
        /// <returns>Returns a list of workouts</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [TypeFilter(typeof(ResultFilter<IEnumerable<WorkoutWithoutSetsDto>>))]
        public async Task<ActionResult<IEnumerable<WorkoutWithoutSetsDto>>> GetWorkouts(string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxWorkoutsPageSize)
            {
                pageSize = maxWorkoutsPageSize;
            }

            var (workouts, paginationMetadata) = await _fitnessAppRepository.GetWorkoutsAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(workouts);
        }

        /// <summary>
        /// Get a workout by id
        /// </summary>
        /// <param name="id">The id of the workout to get</param>
        /// <returns>An IActionResult</returns>
        /// <response code ="200">Returns the requested workout</response>
        [HttpGet("{id}", Name = "GetWorkout")]
        [ProducesResponseType(typeof(WorkoutDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [TypeFilter(typeof(ResultFilter<WorkoutDto>))]
        public async Task<IActionResult> GetWorkout(int id)
        {
            var workout = await _fitnessAppRepository.GetWorkoutAsync(id);

            if (workout == null)
            {
                return NotFound();
            }

            // claims test
            if (workout.Name.ToLower().Contains("nsfw") && !UserIsOfLegalAge(User))
            {
                return Forbid();
            }

            return Ok(workout);
        }

        /// <summary>
        /// Create a workout
        /// </summary>
        /// <param name="workoutForCreationDto">The required data to create a workout</param>
        /// <returns>Returns the newly created workout</returns>
        /// <response code="201">Returns the newly created workout</response>
        /// <response code="409">Returns a conflict in case a workout with this name already exists</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [TypeFilter(typeof(ResultFilter<WorkoutWithoutSetsDto>))]
        public async Task<ActionResult<WorkoutWithoutSetsDto>> CreateWorkout(WorkoutForCreationDto workoutForCreationDto)
        {
            if (await _fitnessAppRepository.WorkoutNameExistsAsync(workoutForCreationDto.Name))
            {
                return Conflict($"Workout with name {workoutForCreationDto.Name} already exists.");
            }

            var newWorkout = _mapper.Map<Workout>(workoutForCreationDto);

            _fitnessAppRepository.CreateWorkout(newWorkout);

            await _fitnessAppRepository.SaveChangesAsync();

            return CreatedAtRoute("GetWorkout",
                new
                {
                    id = newWorkout.Id
                },
                newWorkout);
        }

        private static bool UserIsOfLegalAge(ClaimsPrincipal user)
        {
            const int legalAge = 18;

            var dateOfBirthString = user.Claims.FirstOrDefault(claim => claim.Type == "date_of_birth")?.Value;
            if (DateOnly.TryParse(dateOfBirthString, out var dateOfBirth))
            {
                var today = DateOnly.FromDateTime(DateTime.Now);
                var userIsOfLegalAge = dateOfBirth.AddYears(legalAge) <= today;

                if (userIsOfLegalAge)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
