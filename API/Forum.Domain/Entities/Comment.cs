using Forum.Domain.Common;

namespace Forum.Domain.Entities;

public class Comment : BaseDeletableEntity<int>
{
    public Comment()
    {
        this.Votes = new HashSet<Vote>();
        this.Replies = new HashSet<Comment>();
    }

    public string Content { get; set; }

    public string AuthorId { get; set; }

    public int PostId { get; set; }
    public virtual Post Post { get; set; }

    public int? ParentCommentId { get; set; }
    public virtual Comment ParentComment { get; set; }

    public virtual ICollection<Comment> Replies { get; set; }

    public virtual ICollection<Vote> Votes { get; set; }

    public int VotesCount { get; set; }
}