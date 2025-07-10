using Forum.Data.Contracts.Models;

namespace Forum.Domain.Common;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}

public abstract class BaseDeletableEntity<TKey> : BaseEntity<TKey>, IDeletableEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
}