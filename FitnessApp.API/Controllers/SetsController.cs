using FitnessApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers
{
    [Route("api/workouts/{workoutId}/sets")]
    [ApiController]
    public class SetsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<SetDto>> GetSets(int workoutId)
        {
            var workout = WorkoutsDataStore.Current.Workouts.FirstOrDefault(workout => workout.Id == workoutId);
            if (workout == null)
            {
                return NotFound();
            }

            return Ok(workout.Sets);
        }

        [HttpGet("{setId}", Name = "GetSet")]
        public ActionResult<SetDto> GetSet(int workoutId, int setId)
        {
            var workout = WorkoutsDataStore.Current.Workouts.FirstOrDefault(workout => workout.Id == workoutId);
            if (workout == null)
            {
                return NotFound();
            }

            var set = workout.Sets.FirstOrDefault(set => set.Id == setId);
            if (set == null)
            {
                return NotFound();
            }

            return Ok(set);
        }

        [HttpPost]
        public ActionResult<SetDto> CreateSet(int workoutId, SetForCreationDto set)
        {
            var workout = WorkoutsDataStore.Current.Workouts.FirstOrDefault(workout => workout.Id == workoutId);
            if (workout == null)
            {
                return NotFound();
            }

            var maxSetId = WorkoutsDataStore.Current.Workouts.SelectMany(workout => workout.Sets).Max(set => set.Id);

            var finalSet = new SetDto()
            {
                Id = ++maxSetId,
                Name = set.Name,
                Intensity = set.Intensity
            };

            workout.Sets.Add(finalSet);

            return CreatedAtRoute("GetSet",
                new
                {
                    workoutId = workoutId,
                    setId = finalSet.Id
                }, finalSet
            );
        }

        [HttpPut("{setId}")]
        public ActionResult UpdateSet(int workoutId, int setId, SetForUpdateDto set)
        {
            var workout = WorkoutsDataStore.Current.Workouts.FirstOrDefault(workout => workout.Id == workoutId);
            if (workout == null)
            {
                return NotFound();
            }

            var setFromStore = workout.Sets.FirstOrDefault(set => set.Id == setId);
            if (setFromStore == null)
            {
                return NotFound();
            }

            setFromStore.Name = set.Name;
            setFromStore.Intensity = set.Intensity;

            return NoContent();
        }
    }
}
