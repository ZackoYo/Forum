using Forum.Data.Contracts.Repositories;
using Forum.Data.Models;
using Forum.Infrastructure.Contracts.InternalServices;
using Forum.Infrastructure.Mapping;

namespace Forum.Infrastructure.InternalServices;

public class SettingsService : ISettingsService
{
	private readonly IDeletableEntityRepository<Setting> settingsRepository;

	public SettingsService(IDeletableEntityRepository<Setting> settingsRepository)
	{
		this.settingsRepository = settingsRepository;
	}

	public int GetCount()
	{
		return this.settingsRepository.AllAsNoTracking().Count();
	}

	public IEnumerable<T> GetAll<T>()
	{
		return this.settingsRepository.All().To<T>().ToList();
	}
}