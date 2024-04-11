using Contracts.BLL.Base.Services;
using Contracts.DAL.Base;
using Contracts.DAL.Base.Mappers;
using Contracts.DAL.Base.Repositories;
using Contracts.Domain.Base;

namespace BLL.Base.Services;

public class BaseEntityService<TUnitOfWork, TRepository, TBllEntity, TDalEntity>
    : BaseEntityService<TUnitOfWork, TRepository, TBllEntity, TDalEntity, Guid>,
        IBaseEntityService<TBllEntity, TDalEntity>
    where TBllEntity : class, IDomainEntityId
    where TDalEntity : class, IDomainEntityId
    where TUnitOfWork : IBaseUnitOfWork
    where TRepository : IBaseRepository<TDalEntity>
{
    protected BaseEntityService(TUnitOfWork serviceUow, TRepository serviceRepository,
        IBaseMapper<TBllEntity, TDalEntity> mapper) : base(serviceUow, serviceRepository, mapper)
    {
    }
}

public class
    BaseEntityService<TUnitOfWork, TRepository, TBllEntity, TDalEntity, TKey> : IBaseEntityService<TBllEntity,
    TDalEntity, TKey>
    where TBllEntity : class, IDomainEntityId<TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
    where TUnitOfWork : IBaseUnitOfWork
    where TRepository : IBaseRepository<TDalEntity, TKey>
{
    protected IBaseMapper<TBllEntity, TDalEntity> Mapper;
    protected TRepository ServiceRepository;
    protected TUnitOfWork ServiceUow;

    protected BaseEntityService(TUnitOfWork serviceUow, TRepository serviceRepository,
        IBaseMapper<TBllEntity, TDalEntity> mapper)
    {
        ServiceUow = serviceUow;
        ServiceRepository = serviceRepository;
        Mapper = mapper;
    }

    public TBllEntity Add(TBllEntity entity)
    {
        return Mapper.Map(ServiceRepository.Add(Mapper.Map(entity)!))!;
    }

    public TBllEntity Update(TBllEntity entity)
    {
        return Mapper.Map(ServiceRepository.Update(Mapper.Map(entity)!))!;
    }

    public void UpdateRange(IEnumerable<TBllEntity> entities)
    {
        ServiceRepository.UpdateRange(entities.Select(e => Mapper.Map(e)!));
    }

    public TBllEntity Remove(TBllEntity entity)
    {
        return Mapper.Map(ServiceRepository.Remove(Mapper.Map(entity)!))!;
    }

    public async Task<IEnumerable<TBllEntity>> GetAllAsync()
    {
        return (await ServiceRepository.GetAllAsync()).Select(entity => Mapper.Map(entity))!;
    }

    public async Task<TBllEntity?> FirstOrDefaultAsync(TKey id)
    {
        return Mapper.Map(await ServiceRepository.FirstOrDefaultAsync(id));
    }

    public async Task<bool> ExistsAsync(TKey id)
    {
        return await ServiceRepository.ExistsAsync(id);
    }

    public async Task<TBllEntity> RemoveAsync(TKey id)
    {
        return Mapper.Map(await ServiceRepository.RemoveAsync(id))!;
    }
}