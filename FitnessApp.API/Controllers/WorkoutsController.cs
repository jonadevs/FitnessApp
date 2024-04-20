﻿using System.Text.Json;
using FitnessApp.API.Entities;
using FitnessApp.API.Filters;
using FitnessApp.API.Models;
using FitnessApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers;

[ApiController]
[Authorize]
[Route("api/workouts")]
public class WorkoutsController : ControllerBase
{
    private readonly IFitnessAppRepository _fitnessAppRepository;
    private readonly IWorkoutService _workoutService;
    const int maxPageSize = 20;

    public WorkoutsController(IFitnessAppRepository fitnessAppRepository, IWorkoutService workoutService)
    {
        _fitnessAppRepository = fitnessAppRepository;
        _workoutService = workoutService;
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
        pageSize = pageSize > maxPageSize ? maxPageSize : pageSize;
        var (workouts, paginationMetadata) = await _workoutService.GetWorkoutsAsync(name, searchQuery, pageNumber, pageSize);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
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
        var workout = await _workoutService.GetWorkoutByIdWithSetsAsync(id);
        if (workout == null)
        {
            return NotFound();
        }

        return Ok(workout);
    }

    /// <summary>
    /// Create a workout
    /// </summary>
    /// <param name="createWorkoutDto">The required data to create a workout</param>
    /// <returns>Returns the newly created workout</returns>
    /// <response code="201">Returns the newly created workout</response>
    /// <response code="409">Returns a conflict in case a workout with this name already exists</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [TypeFilter(typeof(ResultFilter<WorkoutWithoutSetsDto>))]
    public async Task<ActionResult<WorkoutWithoutSetsDto>> CreateWorkout(CreateWorkoutDTO createWorkoutDto)
    {
        if (await _workoutService.WorkoutNameExistsAsync(createWorkoutDto.Name))
        {
            return Conflict($"Workout with name {createWorkoutDto.Name} already exists.");
        }

        var workout = new Workout
        {
            Name = createWorkoutDto.Name,
            Type = createWorkoutDto.Type,
            Date = createWorkoutDto.Date,
            Length = createWorkoutDto.Length
        };
        _workoutService.CreateWorkout(workout);
        await _fitnessAppRepository.SaveChangesAsync();

        return CreatedAtRoute("GetWorkout",
            new
            {
                id = workout.Id
            },
            workout);
    }
}
