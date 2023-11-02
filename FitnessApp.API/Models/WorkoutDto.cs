namespace FitnessApp.API.Models
{
    public class WorkoutDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int NumberOfSets { 
            get {
                return Sets.Count;
            }
        }

        public ICollection<SetDto> Sets { get; set; } = new List<SetDto>();
    }
}
