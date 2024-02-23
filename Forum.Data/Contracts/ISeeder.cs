namespace Forum.Data.Contracts;

public interface ISeeder
{
	Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider);
}