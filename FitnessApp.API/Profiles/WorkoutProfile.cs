using AutoMapper;

namespace FitnessApp.API.Profiles;

public class WorkoutProfile : Profile
{
    public WorkoutProfile()
    {
        CreateMap<Entities.Workout, Models.WorkoutWithoutSetsDto>();
        CreateMap<Entities.Workout, Models.WorkoutDto>();

        CreateMap<Models.CreateWorkoutDTO, Entities.Workout>();
    }
}
