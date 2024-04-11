using AutoMapper;

namespace BLL.App.DTO.MappingProfiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Request, DAL.App.DTO.Request>().ReverseMap();
    }
}