using AutoMapper;

namespace FitnessApp.API.Profiles;

public class WorkoutProfile : Profile
{
    public WorkoutProfile()
    {
        CreateMap<Entities.Workout, Models.WorkoutWithoutSetsDTO>();
        CreateMap<Entities.Workout, Models.WorkoutDTO>();
    }
}
