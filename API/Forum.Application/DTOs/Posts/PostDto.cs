namespace Forum.Application.DTOs.Posts;

public class PostDto : BaseDeleteableDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Slug { get; set; }
    public string AuthorId { get; set; }
    public string AuthorName { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int ViewCount { get; set; }
    public int VotesCount { get; set; }
    public int CommentsCount { get; set; }
    public List<string> Tags { get; set; } = new();
}

public class PostListDto : BaseDeleteableDto
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string AuthorName { get; set; }
    public string CategoryName { get; set; }
    public int ViewCount { get; set; }
    public int VotesCount { get; set; }
    public int CommentsCount { get; set; }
}

public class CreatePostRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
    public int CategoryId { get; set; }
    public List<string> Tags { get; set; } = new();
}

public class UpdatePostRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
    public int CategoryId { get; set; }
    public List<string> Tags { get; set; } = new();
}