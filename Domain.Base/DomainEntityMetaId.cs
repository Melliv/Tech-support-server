using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;

namespace Domain.Base;

public class DomainEntityMetaId : DomainEntityMetaId<Guid>, IDomainEntityId
{
}

public class DomainEntityMetaId<TKey> : DomainEntityId<TKey>, IDomainEntityMeta
    where TKey : IEquatable<TKey>
{
    [DataType(DataType.Date)] public DateTime CreateAt { get; set; }

    [DataType(DataType.Date)] public DateTime UpdatedAt { get; set; } = DateTime.Now;
}