using AutoMapper;

namespace FitnessApp.API.Profiles;

public class SetProfile : Profile
{
    public SetProfile()
    {
        CreateMap<Entities.Set, Models.SetDto>();

        CreateMap<Models.SetForCreationDto, Entities.Set>();
        CreateMap<Models.SetForUpdateDto, Entities.Set>();
    }
}
