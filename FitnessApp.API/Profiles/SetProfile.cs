using AutoMapper;

namespace FitnessApp.API.Profiles;

public class SetProfile : Profile
{
    public SetProfile()
    {
        CreateMap<Entities.Set, Models.SetDTO>();
        CreateMap<Models.UpdateSetDTO, Entities.Set>();
    }
}
