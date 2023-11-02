using FitnessApp.API.Models;
using FitnessApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers
{
    [Route("api/workouts/{workoutId}/sets")]
    [ApiController]
    public class SetsController : ControllerBase
    {
        private readonly ILogger<SetsController> _logger;
        private readonly IMailService _mailService;
        private readonly WorkoutsDataStore _workoutsDataStore;

        public SetsController(ILogger<SetsController> logger, IMailService mailService, WorkoutsDataStore workoutsDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _workoutsDataStore = workoutsDataStore;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SetDto>> GetSets(int workoutId)
        {
            try
            {
                var workout = _workoutsDataStore.Workouts.FirstOrDefault(workout => workout.Id == workoutId);
                if (workout == null)
                {
                    _logger.LogInformation($"Workout with id {workoutId} wasn't found when accessing sets.");
                    return NotFound();
                }

                return Ok(workout.Sets);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Exception while getting sets for a workout with id {workoutId}", exception);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{setId}", Name = "GetSet")]
        public ActionResult<SetDto> GetSet(int workoutId, int setId)
        {
            var workout = _workoutsDataStore.Workouts.FirstOrDefault(workout => workout.Id == workoutId);
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
            var workout = _workoutsDataStore.Workouts.FirstOrDefault(workout => workout.Id == workoutId);
            if (workout == null)
            {
                return NotFound();
            }

            var maxSetId = _workoutsDataStore.Workouts.SelectMany(workout => workout.Sets).Max(set => set.Id);

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
            var workout = _workoutsDataStore.Workouts.FirstOrDefault(workout => workout.Id == workoutId);
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

        [HttpDelete("{setId}")]
        public ActionResult DeleteSet(int workoutId, int setId)
        {
            var workout = _workoutsDataStore.Workouts.FirstOrDefault(workout => workout.Id == workoutId);
            if (workout == null)
            {
                return NotFound();
            }

            var set = workout.Sets.FirstOrDefault(set => set.Id == setId);
            if (set == null)
            {
                return NotFound();
            }

            workout.Sets.Remove(set);
            _mailService.Send("Set deleted.", $"Set {set.Name} with id {set.Id} was successfully deleted.");
            return NoContent();
        }
    }
}
