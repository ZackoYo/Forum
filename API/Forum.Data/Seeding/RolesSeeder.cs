﻿using Forum.Common;
using Forum.Data.Contracts;
using Forum.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Data.Seeding;

internal class RolesSeeder : ISeeder
{
	public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
	{
		var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

		await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName);
	}

	private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
	{
		var role = await roleManager.FindByNameAsync(roleName);
		if (role == null)
		{
			var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
			if (!result.Succeeded)
			{
				throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
			}
		}
	}
}