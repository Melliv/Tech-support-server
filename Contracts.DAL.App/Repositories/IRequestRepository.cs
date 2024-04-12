using Contracts.DAL.Base.Repositories;
using Request = DAL.App.DTO.Request;

namespace Contract.DAL.App.Repositories;

public interface IRequestRepository : IBaseRepository<Request>, IRequestRepositoryCustom<Request>
{
}

public interface IRequestRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllUnsolvedAsync();
}