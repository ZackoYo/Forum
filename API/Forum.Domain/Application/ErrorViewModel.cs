﻿namespace Forum.Domain.Application;

public class ErrorViewModel
{
	public string RequestId { get; set; }

	public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
}