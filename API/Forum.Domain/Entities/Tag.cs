using Forum.Domain.Common;

namespace Forum.Domain.Entities;

public class Tag : BaseDeletableEntity<int>
{
    public Tag()
    {
        this.Posts = new HashSet<Post>();
    }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Slug { get; set; }

    public virtual ICollection<Post> Posts { get; set; }

    public int PostsCount { get; set; }
}