namespace Forum.Application.DTOs.Comments;

public class CommentDto : BaseDeleteableDto
{
    public string Content { get; set; }
    public string AuthorId { get; set; }
    public string AuthorName { get; set; }
    public int PostId { get; set; }
    public int? ParentCommentId { get; set; }
    public int VotesCount { get; set; }
    public List<CommentDto> Replies { get; set; } = new();
}

public class CreateCommentRequest
{
    public string Content { get; set; }
    public int PostId { get; set; }
    public int? ParentCommentId { get; set; }
}

public class UpdateCommentRequest
{
    public string Content { get; set; }
}