﻿using Forum.Data.Contracts.Models;
using Microsoft.AspNetCore.Identity;

namespace Forum.Data.Models;

public class ApplicationRole : IdentityRole, IAuditInfo, IDeletableEntity
{
	public ApplicationRole()
		: this(null)
	{
	}

	public ApplicationRole(string name)
		: base(name)
	{
		this.Id = Guid.NewGuid().ToString();
	}

	public DateTime CreatedOn { get; set; }

	public DateTime? ModifiedOn { get; set; }

	public bool IsDeleted { get; set; }

	public DateTime? DeletedOn { get; set; }
}