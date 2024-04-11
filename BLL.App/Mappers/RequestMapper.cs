using AutoMapper;
using BLL.App.DTO;

namespace BLL.App.Mappers;

public class RequestMapper(IMapper mapper) : BaseMapper<Request, DAL.App.DTO.Request>(mapper);