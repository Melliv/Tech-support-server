using Contracts.DAL.Base;

namespace DAL.Base;

public abstract class BaseUnitOfWork : IBaseUnitOfWork
{
    private readonly Dictionary<Type, object> _repoCache = new();
    public abstract Task<int> SaveChangesAsync();

    public abstract int SaveChanges();

    public TRepository GetRepository<TRepository>(Func<TRepository> repoCreationMethod)
        where TRepository : class
    {
        if (_repoCache.TryGetValue(typeof(TRepository), out var repo)) return (TRepository)repo;

        var repoInstance = repoCreationMethod();
        _repoCache.Add(typeof(TRepository), repoInstance);
        return repoInstance;
    }
}