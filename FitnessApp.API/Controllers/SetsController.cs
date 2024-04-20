using AutoMapper;
using FitnessApp.API.Entities;
using FitnessApp.API.Models;
using FitnessApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers;

[ApiController]
[Route("api/workouts/{workoutId}/sets")]
public class SetsController : ControllerBase
{
    private readonly ILogger<SetsController> _logger;
    private readonly IFitnessAppRepository _fitnessAppRepository;
    private readonly IMapper _mapper;

    public SetsController(ILogger<SetsController> logger, IFitnessAppRepository fitnessAppRepository, IMapper mapper)
    {
        _logger = logger;
        _fitnessAppRepository = fitnessAppRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SetDTO>>> GetSets(int workoutId)
    {
        if (!await _fitnessAppRepository.WorkoutExistsAsync(workoutId))
        {
            _logger.LogInformation("Workout with id {workoutId} wasn't found when accessing sets.", workoutId);
            return NotFound();
        }

        var setsForWorkout = await _fitnessAppRepository.GetSetsForWorkoutAsync(workoutId);
        return Ok(_mapper.Map<IEnumerable<SetDTO>>(setsForWorkout));
    }

    [HttpGet("{setId}", Name = "GetSet")]
    public async Task<ActionResult<SetDTO>> GetSet(int workoutId, int setId)
    {
        if (!await _fitnessAppRepository.WorkoutExistsAsync(workoutId))
        {
            _logger.LogInformation("Workout with id {workoutId} wasn't found when accessing sets.", workoutId);
            return NotFound();
        }

        var setForWorkout = await _fitnessAppRepository.GetSetForWorkoutAsync(workoutId, setId);
        if (setForWorkout == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<SetDTO>(setForWorkout));
    }

    [HttpPost]
    public async Task<ActionResult<SetDTO>> CreateSet(int workoutId, CreateSetDTO createSetDto)
    {
        if (!await _fitnessAppRepository.WorkoutExistsAsync(workoutId))
        {
            _logger.LogInformation("Workout with id {workoutId} wasn't found when creating a set.", workoutId);
            return NotFound();
        }

        var set = new Set
        {
            Intensity = createSetDto.Intensity,
            Name = createSetDto.Name,
        };
        await _fitnessAppRepository.AddSetForWorkoutAsync(workoutId, set);
        await _fitnessAppRepository.SaveChangesAsync();

        var setDto = _mapper.Map<SetDTO>(set);
        return CreatedAtRoute("GetSet",
            new
            {
                workoutId,
                setId = setDto.Id
            },
            setDto
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

        return NoContent();
    }
}
