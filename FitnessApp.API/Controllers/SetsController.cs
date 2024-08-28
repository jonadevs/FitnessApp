using AutoMapper;
using FitnessApp.API.Entities;
using FitnessApp.API.Models;
using FitnessApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers;

[ApiController]
[Route("api/workouts/{workoutId}/sets")]
public class SetsController(
    ILogger<SetsController> logger,
    IFitnessAppRepository fitnessAppRepository,
    IMapper mapper, IWorkoutService workoutService,
    ISetService setService)
    : ControllerBase
{
    private readonly ILogger<SetsController> _logger = logger;
    private readonly IFitnessAppRepository _fitnessAppRepository = fitnessAppRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IWorkoutService _workoutService = workoutService;
    private readonly ISetService _setService = setService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SetDTO>>> GetSets(int workoutId)
    {
        if (!await _workoutService.WorkoutExistsAsync(workoutId))
        {
            _logger.LogInformation("Workout with id {WorkoutId} wasn't found.", workoutId);
            return NotFound();
        }

        var sets = await _setService.GetSetsForWorkoutAsync(workoutId);

        var setDtos = _mapper.Map<IEnumerable<SetDTO>>(sets);
        return Ok(setDtos);
    }

    [HttpGet("{setId}", Name = "GetSet")]
    public async Task<ActionResult<SetDTO>> GetSetById(int workoutId, int setId)
    {
        if (!await _workoutService.WorkoutExistsAsync(workoutId))
        {
            _logger.LogInformation("Workout with id {WorkoutId} wasn't found.", workoutId);
            return NotFound();
        }

        var sets = await _setService.GetSetForWorkoutAsync(workoutId, setId);
        if (sets is null)
        {
            return NotFound();
        }

        var setDto = _mapper.Map<SetDTO>(sets);
        return Ok(setDto);
    }

    [HttpPost]
    public async Task<ActionResult<SetDTO>> CreateSet(int workoutId, CreateSetDTO createSetDto)
    {
        if (!await _workoutService.WorkoutExistsAsync(workoutId))
        {
            _logger.LogInformation("Workout with id {WorkoutId} wasn't found.", workoutId);
            return NotFound();
        }

        var set = new Set
        {
            Intensity = createSetDto.Intensity,
            Name = createSetDto.Name,
        };
        await _setService.AddSetForWorkoutAsync(workoutId, set);
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
    public async Task<ActionResult> UpdateSet(int workoutId, int setId, UpdateSetDTO updateSetDto)
    {
        if (!await _workoutService.WorkoutExistsAsync(workoutId))
        {
            _logger.LogInformation("Workout with id {WorkoutId} wasn't found.", workoutId);
            return NotFound();
        }

        var set = await _setService.GetSetForWorkoutAsync(workoutId, setId);
        if (set is null)
        {
            _logger.LogInformation("Set with id {SetId} wasn't found.", setId);
            return NotFound();
        }

        set.Name = updateSetDto.Name;
        set.Intensity = updateSetDto.Intensity;
        await _fitnessAppRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{setId}")]
    public async Task<ActionResult> DeleteSet(int workoutId, int setId)
    {
        if (!await _workoutService.WorkoutExistsAsync(workoutId))
        {
            _logger.LogInformation("Workout with id {WorkoutId} wasn't found.", workoutId);
            return NotFound();
        }

        var set = await _setService.GetSetForWorkoutAsync(workoutId, setId);
        if (set is null)
        {
            _logger.LogInformation("Set with id {SetId} wasn't found.", setId);
            return NotFound();
        }

        _setService.DeleteSet(set);
        await _fitnessAppRepository.SaveChangesAsync();

        return NoContent();
    }
}
