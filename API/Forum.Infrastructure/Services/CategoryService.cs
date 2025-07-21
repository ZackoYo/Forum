using Forum.Application.DTOs.Categories;
using Forum.Application.Services;
using Forum.Data.Contracts.Repositories;
using Forum.Domain.Entities;
using Forum.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly IDeletableEntityRepository<Category> _categoryRepository;

    public CategoryService(IDeletableEntityRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public IEnumerable<T> GetAllCategories<T>(int? count = null)
    {
        IQueryable<Category> categories = _categoryRepository
            .AllAsNoTracking()
            .Where(c => c.ParentCategoryId == null)
            .Include(c => c.SubCategories)
            .OrderBy(c => c.DisplayOrder);

        if (count.HasValue)
        {
            categories = categories.Take(count.Value);
        }

        return categories.To<T>().ToList();
    }

    public T GetByName<T>(string name)
    {
        var category = _categoryRepository.All()
            .Where(x => x.Name.Replace(" ", "-") == name.Replace(" ", "-"))
            .To<T>().FirstOrDefault();

        return category;
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
        var categoryDto = await _categoryRepository
            .AllAsNoTracking()
            .Where(c => c.Id == id)
            .To<CategoryDto>()
            .FirstOrDefaultAsync();

        if (categoryDto == null)
            throw new KeyNotFoundException($"Category with ID {id} not found.");

        return categoryDto;
    }

    public async Task<CategoryDto> GetCategoryBySlugAsync(string slug)
    {
        var categoryDto = await _categoryRepository
            .AllAsNoTracking()
            .Where(c => c.Slug == slug)
            .To<CategoryDto>()
            .FirstOrDefaultAsync();

        if (categoryDto == null)
            throw new KeyNotFoundException($"Category with slug {slug} not found.");

        return categoryDto;
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description,
            ParentCategoryId = request.ParentCategoryId,
            DisplayOrder = request.DisplayOrder,
            Slug = request.Name.ToLower().Replace(" ", "-")
        };

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();

        return MapToDto(category);
    }

    public async Task<CategoryDto> UpdateCategoryAsync(int id, UpdateCategoryRequest request)
    {
        var category = await _categoryRepository
            .All()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            throw new KeyNotFoundException($"Category with ID {id} not found.");

        category.Name = request.Name;
        category.Description = request.Description;
        category.ParentCategoryId = request.ParentCategoryId;
        category.DisplayOrder = request.DisplayOrder;
        category.Slug = request.Name.ToLower().Replace(" ", "-");

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync();

        return MapToDto(category);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepository
            .All()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            throw new KeyNotFoundException($"Category with ID {id} not found.");

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync();
    }

    private static CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Slug = category.Slug,
            ParentCategoryId = category.ParentCategoryId,
            PostsCount = category.PostsCount,
            DisplayOrder = category.DisplayOrder,
            CreatedOn = category.CreatedOn,
            ModifiedOn = category.ModifiedOn,
            IsDeleted = category.IsDeleted,
            DeletedOn = category.DeletedOn,
            SubCategories = category.SubCategories?.Select(MapToDto).ToList() ?? new List<CategoryDto>()
        };
    }
}