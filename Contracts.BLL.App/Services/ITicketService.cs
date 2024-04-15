using BLL.App.DTO;
using Contract.DAL.App.Repositories;
using Contracts.BLL.Base.Services;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.BLL.App.Services;

public interface ITicketService : IBaseEntityService<Ticket, DALAppDTO.Ticket>, ITicketRepositoryCustom<Ticket>
{
    new Task<IEnumerable<Ticket>> GetAllUnsolvedAsync();
}