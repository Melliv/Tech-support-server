using AutoMapper;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contract.DAL.App;
using Contract.DAL.App.Repositories;
using Contracts.BLL.App.Services;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace BLL.App.Services;

public class RequestService :
    BaseEntityService<IAppUnitOfWork, IRequestRepository, BLLAppDTO.Request, DALAppDTO.Request>, IRequestService
{
    public RequestService(IAppUnitOfWork serviceUow, IRequestRepository serviceRepository, IMapper mapper) : base(
        serviceUow, serviceRepository, new RequestMapper(mapper))
    {
    }

    public new BLLAppDTO.Request Add(BLLAppDTO.Request entity)
    {
        entity.CreateAt = entity.UpdatedAt;
        return base.Add(entity);
    }

    public new BLLAppDTO.Request Update(BLLAppDTO.Request entity)
    {
        entity.UpdatedAt = DateTime.Now;
        return base.Update(entity);
    }

    public async Task<IEnumerable<BLLAppDTO.Request>> GetAllUnsolvedAsync()
    {
        return (await ServiceRepository.GetAllUnsolvedAsync()).Select(x => Mapper.Map(x))!;
    }
}