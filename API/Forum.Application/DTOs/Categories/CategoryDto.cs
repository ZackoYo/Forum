namespace Forum.Application.DTOs.Categories;

public class CategoryDto : BaseDeleteableDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public int? ParentCategoryId { get; set; }
    public int PostsCount { get; set; }
    public int DisplayOrder { get; set; }
    public List<CategoryDto> SubCategories { get; set; } = new();
}

public class CreateCategoryRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public int DisplayOrder { get; set; }
}

public class UpdateCategoryRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public int DisplayOrder { get; set; }
}