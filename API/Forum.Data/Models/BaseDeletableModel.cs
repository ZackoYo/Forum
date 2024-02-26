using Forum.Data.Contracts.Models;

namespace Forum.Data.Models;

public abstract class BaseDeletableModel<TKey> : BaseModel<TKey>, IDeletableEntity
{
	public bool IsDeleted { get; set; }

	public DateTime? DeletedOn { get; set; }
}