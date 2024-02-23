namespace Forum.Data.Contracts.Models;

public interface IAuditInfo
{
	DateTime CreatedOn { get; set; }

	DateTime? ModifiedOn { get; set; }
}