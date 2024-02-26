namespace Forum.Infrastructure.Contracts.InternalServices;

public interface ISettingsService
{
	int GetCount();

	IEnumerable<T> GetAll<T>();
}