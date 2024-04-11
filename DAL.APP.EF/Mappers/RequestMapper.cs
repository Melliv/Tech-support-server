using AutoMapper;
using DAL.App.DTO;

namespace DAL.APP.EF.Mappers;

public class RequestMapper(IMapper mapper) : BaseMapper<Request, Domain.App.Request>(mapper);