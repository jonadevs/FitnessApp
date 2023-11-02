using FitnessApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers
{
    [ApiController]
    [Route("api/workouts")]
    public class WorkoutsController : ControllerBase
    {
        private readonly WorkoutsDataStore _workoutsDataStore;

        public WorkoutsController(WorkoutsDataStore workoutsDataStore)
        {
            _workoutsDataStore = workoutsDataStore;
        }

        [HttpGet]
        public ActionResult<IEnumerable<WorkoutDto>> GetWorkouts()
        {
            return Ok(_workoutsDataStore.Workouts);
        }

        [HttpGet("{id}")]
        public ActionResult<WorkoutDto> GetWorkout(int id)
        {
            var cityToReturn = _workoutsDataStore.Workouts.FirstOrDefault(workout => workout.Id == id);

            if(cityToReturn == null)
            {
                return NotFound();
            }

            return Ok(cityToReturn);
        }
    }
}
