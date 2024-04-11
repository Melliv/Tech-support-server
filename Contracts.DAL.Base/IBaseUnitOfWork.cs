namespace Contracts.DAL.Base;

public interface IBaseUnitOfWork
{
    Task<int> SaveChangesAsync();

    int SaveChanges();

    TRepository GetRepository<TRepository>(Func<TRepository> repoCreationMethod)
        where TRepository : class;
}