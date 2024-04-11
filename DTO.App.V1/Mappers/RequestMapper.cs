using AutoMapper;

namespace DTO.App.V1.Mappers;

public class RequestMapper(IMapper mapper) : BaseMapper<BLL.App.DTO.Request, Request>(mapper);