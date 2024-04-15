using AutoMapper;

namespace DTO.App.V1.MappingProfiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<BLL.App.DTO.Ticket, Ticket>().ReverseMap();
    }
}