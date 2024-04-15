using AutoMapper;

namespace BLL.App.DTO.MappingProfiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Ticket, DAL.App.DTO.Ticket>().ReverseMap();
    }
}