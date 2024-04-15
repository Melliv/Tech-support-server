using AutoMapper;

namespace DTO.App.V1.Mappers;

public class TicketMapper(IMapper mapper) : BaseMapper<BLL.App.DTO.Ticket, Ticket>(mapper);