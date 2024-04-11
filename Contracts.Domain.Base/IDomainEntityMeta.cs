namespace Contracts.Domain.Base;

public interface IDomainEntityMeta
{
    DateTime CreateAt { get; set; }
    DateTime UpdatedAt { get; set; }
}