using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.API.Entities
{
    public enum WorkoutType
    {
        WeightTraining,
        Running,
        Swimming,
        Yoga,
        Pilates
    }

    public class Workout
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public WorkoutType Type { get; set; }

        public DateTime Date { get; set; }

        public int Length { get; set; }

        public ICollection<Set> Sets { get; set; }
            = new List<Set>();

        public Workout(string name, WorkoutType type, DateTime date, int length)
        {
            Name = name;
            Type = type;
            Date = date;
            Length = length;
        }
    }
}
