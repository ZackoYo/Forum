﻿namespace Forum.Data.Contracts.Models;

public interface IDeletableEntity
{
	bool IsDeleted { get; set; }

	DateTime? DeletedOn { get; set; }
}