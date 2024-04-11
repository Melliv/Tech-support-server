using Contracts.Domain.Base;

namespace Contracts.DAL.Base.Repositories;

public interface IBaseRepositoryCommon<TEntity, TKey>
    where TEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{
    TEntity Add(TEntity entity);
    TEntity Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entity);
    TEntity Remove(TEntity entity);
}