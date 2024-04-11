using AutoMapper;

namespace DAL.App.DTO.MappingProfiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Request, Domain.App.Request>().ReverseMap();
    }
}