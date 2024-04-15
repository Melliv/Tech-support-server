using Contracts.DAL.Base.Repositories;
using Ticket = DAL.App.DTO.Ticket;

namespace Contract.DAL.App.Repositories;

public interface ITicketRepository : IBaseRepository<Ticket>, ITicketRepositoryCustom<Ticket>
{
}

public interface ITicketRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllUnsolvedAsync();
}