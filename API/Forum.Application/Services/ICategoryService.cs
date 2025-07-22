using Forum.Application.DTOs.Categories;

namespace Forum.Application.Services;

public interface ICategoryService
{
    IEnumerable<T> GetAllCategories<T>(int? count = null);
    T GetByName<T>(string name);
    Task<CategoryDto> GetCategoryByIdAsync(int id);
    Task<CategoryDto> GetCategoryBySlugAsync(string slug);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request);
    Task<CategoryDto> UpdateCategoryAsync(int id, UpdateCategoryRequest request);
}