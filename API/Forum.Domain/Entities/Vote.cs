using Forum.Domain.Common;

namespace Forum.Domain.Entities;

public class Vote : BaseEntity<int>
{
    public string UserId { get; set; }

    public int? PostId { get; set; }
    public virtual Post Post { get; set; }

    public int? CommentId { get; set; }
    public virtual Comment Comment { get; set; }

    public VoteType Type { get; set; }
}

public enum VoteType
{
    Up = 1,
    Down = -1
}