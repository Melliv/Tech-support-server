using AutoMapper;
using BLL.App.DTO;

namespace BLL.App.Mappers;

public class TicketMapper(IMapper mapper) : BaseMapper<Ticket, DAL.App.DTO.Ticket>(mapper);