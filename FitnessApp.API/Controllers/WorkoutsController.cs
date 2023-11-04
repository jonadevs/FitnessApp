using System.Security.Claims;
using System.Text.Json;
using AutoMapper;
using FitnessApp.API.Entities;
using FitnessApp.API.Models;
using FitnessApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/workouts")]
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutWithoutSetsDto>>> GetWorkouts(string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxWorkoutsPageSize)
            {
                pageSize = maxWorkoutsPageSize;
            }

            var (workoutEntities, paginationMetadata) = await _fitnessAppRepository.GetWorkoutsAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<WorkoutWithoutSetsDto>>(workoutEntities));
        }

        [HttpGet("{id}", Name = "GetWorkout")]
        public async Task<IActionResult> GetWorkout(int id, bool includeSets = false)
        {
            var workout = await _fitnessAppRepository.GetWorkoutAsync(id, includeSets);

            if (workout == null)
            {
                return NotFound();
            }

            // claims test
            if (workout.Name.ToLower().Contains("nsfw") && !UserIsOfLegalAge(User))
            {
                return Forbid();
            }

            if (includeSets)
            {
                return Ok(_mapper.Map<WorkoutDto>(workout));
            }

            return Ok(_mapper.Map<WorkoutWithoutSetsDto>(workout));
        }

        [HttpPost]
        public async Task<ActionResult<WorkoutWithoutSetsDto>> CreateWorkout(WorkoutForCreationDto workoutForCreationDto)
        {
            if (await _fitnessAppRepository.WorkoutNameExistsAsync(workoutForCreationDto.Name))
            {
                return Conflict($"Workout with name {workoutForCreationDto.Name} already exists.");
            }

            var newWorkout = _mapper.Map<Workout>(workoutForCreationDto);

            _fitnessAppRepository.CreateWorkout(newWorkout);

            await _fitnessAppRepository.SaveChangesAsync();

            var createdWorkoutToReturn = _mapper.Map<WorkoutWithoutSetsDto>(newWorkout);

            return CreatedAtRoute("GetWorkout",
                new
                {
                    id = createdWorkoutToReturn.Id
                },
                createdWorkoutToReturn);
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
