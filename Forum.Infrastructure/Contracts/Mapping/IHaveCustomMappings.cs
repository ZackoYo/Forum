using AutoMapper;

namespace Forum.Infrastructure.Contracts.Mapping;

public interface IHaveCustomMappings
{
	void CreateMappings(IProfileExpression configuration);
}