using AutoMapper;

namespace DAL.App.DTO.MappingProfiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Ticket, Domain.App.Ticket>().ReverseMap();
    }
}