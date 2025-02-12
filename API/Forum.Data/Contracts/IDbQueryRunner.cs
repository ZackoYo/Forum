﻿namespace Forum.Data.Contracts;

public interface IDbQueryRunner : IDisposable
{
	Task RunQueryAsync(string query, params object[] parameters);
}