using AutoMapper;
using DAL.App.DTO;

namespace DAL.APP.EF.Mappers;

public class TicketMapper(IMapper mapper) : BaseMapper<Ticket, Domain.App.Ticket>(mapper);