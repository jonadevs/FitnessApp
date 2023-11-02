using FitnessApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers
{
    [ApiController]
    [Route("api/workouts")]
    public class WorkoutsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<WorkoutDto>> GetWorkouts()
        {
            return Ok(WorkoutsDataStore.Current.Workouts);
        }

        [HttpGet("{id}")]
        public ActionResult<WorkoutDto> GetWorkout(int id)
        {
            var cityToReturn = WorkoutsDataStore.Current.Workouts.FirstOrDefault(workout => workout.Id == id);

            if(cityToReturn == null)
            {
                return NotFound();
            }

            return Ok(cityToReturn);
        }
    }
}
