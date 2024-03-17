using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Forum.Data
{
	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			// Get the directory path where the appsettings.json file resides
			var appSettingsPath = @"C:\Git Repos\Forum\API\Forum.Web";

			// Create a configuration builder and set the base path
			var configuration = new ConfigurationBuilder()
				.SetBasePath(appSettingsPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			// Build the DbContextOptions using the configuration
			var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
			var connectionString = configuration.GetConnectionString("DefaultConnection");
			builder.UseSqlServer(connectionString);

			// Create and return the ApplicationDbContext
			return new ApplicationDbContext(builder.Options);
		}
	}
}
