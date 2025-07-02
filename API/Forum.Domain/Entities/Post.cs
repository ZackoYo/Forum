using Forum.Data.Models;
using Forum.Domain.Common;

namespace Forum.Domain.Entities;

public class Post : BaseDeletableEntity<int>
{
    public Post()
    {
        this.Comments = new HashSet<Comment>();
        this.Votes = new HashSet<Vote>();
        this.Tags = new HashSet<Tag>();
    }

    public string Title { get; set; }
    public string Content { get; set; }
    public string Slug { get; set; }
    public string AuthorId { get; set; }
    public virtual ApplicationUser Author { get; set; }
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Vote> Votes { get; set; }
    public virtual ICollection<Tag> Tags { get; set; }
    public int ViewCount { get; set; }
    public int VotesCount { get; set; }
    public int CommentsCount { get; set; }
}