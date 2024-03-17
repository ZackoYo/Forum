using Forum.Data.Contracts.Models;
using Forum.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Forum.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
	private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
		typeof(ApplicationDbContext).GetMethod(
			nameof(SetIsDeletedQueryFilter),
			BindingFlags.NonPublic | BindingFlags.Static);

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<Setting> Settings { get; set; }

	public override int SaveChanges() => this.SaveChanges(true);

	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		this.ApplyAuditInfoRules();
		return base.SaveChanges(acceptAllChangesOnSuccess);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
		this.SaveChangesAsync(true, cancellationToken);

	public override Task<int> SaveChangesAsync(
		bool acceptAllChangesOnSuccess,
		CancellationToken cancellationToken = default)
	{
		this.ApplyAuditInfoRules();
		return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		// Needed for Identity models configuration
		base.OnModelCreating(builder);

		this.ConfigureUserIdentityRelations(builder);

		EntityIndexesConfiguration.Configure(builder);

		var entityTypes = builder.Model.GetEntityTypes().ToList();

		// Set global query filter for not deleted entities only
		var deletableEntityTypes = entityTypes
			.Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
		foreach (var deletableEntityType in deletableEntityTypes)
		{
			var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
			method.Invoke(null, new object[] { builder });
		}

		// Disable cascade delete
		var foreignKeys = entityTypes
			.SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
		foreach (var foreignKey in foreignKeys)
		{
			foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
		}

		// Customize table names for Identity entities
		builder.Entity<ApplicationUser>().ToTable("Users");
		builder.Entity<ApplicationRole>().ToTable("Roles");
		builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
		builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
		builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
		builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
		builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
	}

	private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
		where T : class, IDeletableEntity
	{
		builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
	}

	// Applies configurations
	private void ConfigureUserIdentityRelations(ModelBuilder builder)
		 => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

	private void ApplyAuditInfoRules()
	{
		var changedEntries = this.ChangeTracker
			.Entries()
			.Where(e =>
				e.Entity is IAuditInfo &&
				(e.State == EntityState.Added || e.State == EntityState.Modified));

		foreach (var entry in changedEntries)
		{
			var entity = (IAuditInfo)entry.Entity;
			if (entry.State == EntityState.Added && entity.CreatedOn == default)
			{
				entity.CreatedOn = DateTime.UtcNow;
			}
			else
			{
				entity.ModifiedOn = DateTime.UtcNow;
			}
		}
	}
}