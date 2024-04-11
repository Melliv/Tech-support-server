using Contracts.DAL.Base.Mappers;
using Contracts.DAL.Base.Repositories;
using Contracts.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base.EF.Repositories;

public class BaseRepository<TDalEntity, TDomainEntity, TDbContext> :
    BaseRepository<TDalEntity, TDomainEntity, Guid, TDbContext>,
    IBaseRepository<TDalEntity>
    where TDalEntity : class, IDomainEntityId
    where TDomainEntity : class, IDomainEntityId
    where TDbContext : DbContext
{
    protected BaseRepository(TDbContext dbContext, IBaseMapper<TDalEntity, TDomainEntity> mapper) : base(dbContext,
        mapper)
    {
    }
}

public class
    BaseRepository<TDalEntity, TDomainEntity, TKey, TDbContext> : IBaseRepository<TDalEntity, TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TDomainEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : DbContext
{
    protected readonly IBaseMapper<TDalEntity, TDomainEntity> Mapper;
    protected readonly TDbContext RepoDbContext;
    private readonly DbSet<TDomainEntity> RepoDbSet;

    protected BaseRepository(TDbContext dbContext, IBaseMapper<TDalEntity, TDomainEntity> mapper)
    {
        RepoDbContext = dbContext;
        RepoDbSet = dbContext.Set<TDomainEntity>();
        Mapper = mapper;
    }

    public virtual async Task<IEnumerable<TDalEntity>> GetAllAsync()
    {
        var query = CreateQuery();
        var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));
        var res = await resQuery.ToListAsync();

        return res!;
    }

    public virtual async Task<TDalEntity?> FirstOrDefaultAsync(TKey id)
    {
        var query = CreateQuery();
        return Mapper.Map(await query.FirstOrDefaultAsync(e => e!.Id.Equals(id)));
    }

    public virtual TDalEntity Add(TDalEntity entity)
    {
        return Mapper.Map(RepoDbSet.Add(Mapper.Map(entity)!).Entity)!;
    }

    public virtual TDalEntity Update(TDalEntity entity)
    {
        return Mapper.Map(RepoDbSet.Update(Mapper.Map(entity)!).Entity)!;
    }

    public virtual void UpdateRange(IEnumerable<TDalEntity> entities)
    {
        RepoDbSet.UpdateRange(entities.Select(e => Mapper.Map(e)!));
    }

    public virtual TDalEntity Remove(TDalEntity entity)
    {
        return Mapper.Map(RepoDbSet.Remove(Mapper.Map(entity)!).Entity)!;
    }

    public virtual async Task<TDalEntity> RemoveAsync(TKey id)
    {
        var entity = await FirstOrDefaultAsync(id);
        if (entity == null)
            throw new NullReferenceException($"Entity {typeof(TDalEntity).Name} with id {id} not found.");
        return Remove(entity!);
    }

    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        return await RepoDbSet.AnyAsync(e => e.Id.Equals(id));
    }

    protected IQueryable<TDomainEntity> CreateQuery()
    {
        var query = RepoDbSet.AsQueryable();

        return query;
    }
}