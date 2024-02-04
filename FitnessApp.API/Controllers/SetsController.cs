using AutoMapper;
using FitnessApp.API.Entities;
using FitnessApp.API.Models;
using FitnessApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers
{
    [ApiController]
    [Authorize(Policy = "MustBeFromBerlin")]
    [Route("api/workouts/{workoutId}/sets")]
    public class SetsController : ControllerBase
    {
        private readonly ILogger<SetsController> _logger;
        private readonly IMailService _mailService;
        private readonly IFitnessAppRepository _fitnessAppRepository;
        private readonly IMapper _mapper;

        public SetsController(ILogger<SetsController> logger, IMailService mailService, IFitnessAppRepository fitnessAppRepository, IMapper mapper)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? 
                throw new ArgumentNullException(nameof(mailService));
            _fitnessAppRepository = fitnessAppRepository ??
                throw new ArgumentNullException(nameof(fitnessAppRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SetDto>>> GetSets(int workoutId)
        {
            if(!await _fitnessAppRepository.WorkoutExistsAsync(workoutId))
            {
                _logger.LogInformation("Workout with id {workoutId} wasn't found when accessing sets.", workoutId);
                return NotFound();
            }

            var setsForWorkout = await _fitnessAppRepository.GetSetsForWorkoutAsync(workoutId);

            return Ok(_mapper.Map<IEnumerable<SetDto>>(setsForWorkout));
        }

        [HttpGet("{setId}", Name = "GetSet")]
        public async Task<ActionResult<SetDto>> GetSet(int workoutId, int setId)
        {
            if (!await _fitnessAppRepository.WorkoutExistsAsync(workoutId))
            {
                _logger.LogInformation("Workout with id {workoutId} wasn't found when accessing sets.", workoutId);
                return NotFound();
            }

            var setForWorkout = await _fitnessAppRepository.GetSetForWorkoutAsync(workoutId, setId);

            if(setForWorkout == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SetDto>(setForWorkout));
        }

        [HttpPost]
        public async Task<ActionResult<SetDto>> CreateSet(int workoutId, SetForCreationDto set)
        {
            if (!await _fitnessAppRepository.WorkoutExistsAsync(workoutId))
            {
                _logger.LogInformation("Workout with id {workoutId} wasn't found when creating a set.", workoutId);
                return NotFound();
            }

            var finalSet = _mapper.Map<Set>(set);

            await _fitnessAppRepository.AddSetForWorkoutAsync(workoutId, finalSet);

            await _fitnessAppRepository.SaveChangesAsync();

            var createdSetToReturn = _mapper.Map<SetDto>(finalSet);

            return CreatedAtRoute("GetSet",
                new
                {
                    workoutId,
                    setId = createdSetToReturn.Id
                }, 
                createdSetToReturn
            );
        }

        [HttpPut("{setId}")]
        public async Task<ActionResult> UpdateSet(int workoutId, int setId, SetForUpdateDto set)
        {
            if (!await _fitnessAppRepository.WorkoutExistsAsync(workoutId))
            {
                _logger.LogInformation("Workout with id {workoutId} wasn't found when updating a set.", workoutId);
                return NotFound();
            }

            var setEntity = await _fitnessAppRepository.GetSetForWorkoutAsync(workoutId, setId);
            if (setEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(set, setEntity);

            await _fitnessAppRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{setId}")]
        public async Task<ActionResult> DeleteSet(int workoutId, int setId)
        {
            if (!await _fitnessAppRepository.WorkoutExistsAsync(workoutId))
            {
                _logger.LogInformation("Workout with id {workoutId} wasn't found when updating a set.", workoutId);
                return NotFound();
            }

            var setEntity = await _fitnessAppRepository.GetSetForWorkoutAsync(workoutId, setId);
            if (setEntity == null)
            {
                return NotFound();
            }

            _fitnessAppRepository.DeleteSet(setEntity);
            await _fitnessAppRepository.SaveChangesAsync();

            _mailService.Send("Set deleted.", $"Set {setEntity.Name} with id {setEntity.Id} was successfully deleted.");

            return NoContent();
        }
    }
}
