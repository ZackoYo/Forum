using Forum.Domain.Common;
using Forum.Data.Contracts.Models;

namespace Forum.Domain.Entities;

public class Category : BaseDeletableEntity<int>
{
    public Category()
    {
        this.Posts = new HashSet<Post>();
        this.SubCategories = new HashSet<Category>();
    }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Slug { get; set; }

    public int? ParentCategoryId { get; set; }
    public Category ParentCategory { get; set; }

    public virtual ICollection<Category> SubCategories { get; set; }

    public virtual ICollection<Post> Posts { get; set; }

    public int PostsCount { get; set; }

    public int DisplayOrder { get; set; }
}