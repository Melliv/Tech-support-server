using AutoMapper;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contract.DAL.App;
using Contract.DAL.App.Repositories;
using Contracts.BLL.App.Services;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace BLL.App.Services;

public class TicketService :
    BaseEntityService<IAppUnitOfWork, ITicketRepository, BLLAppDTO.Ticket, DALAppDTO.Ticket>, ITicketService
{
    public TicketService(IAppUnitOfWork serviceUow, ITicketRepository serviceRepository, IMapper mapper) : base(
        serviceUow, serviceRepository, new TicketMapper(mapper))
    {
    }

    public new BLLAppDTO.Ticket Add(BLLAppDTO.Ticket entity)
    {
        entity.CreateAt = entity.UpdatedAt;
        return base.Add(entity);
    }

    public new BLLAppDTO.Ticket Update(BLLAppDTO.Ticket entity)
    {
        entity.UpdatedAt = DateTime.Now;
        return base.Update(entity);
    }

    public async Task<IEnumerable<BLLAppDTO.Ticket>> GetAllUnsolvedAsync()
    {
        return (await ServiceRepository.GetAllUnsolvedAsync()).Select(x => Mapper.Map(x))!;
    }
}