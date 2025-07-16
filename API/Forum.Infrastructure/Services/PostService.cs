using Forum.Application.DTOs.Posts;
using Forum.Application.DTOs.Comments;
using Forum.Application.Services;
using Forum.Domain.Entities;

namespace Forum.Infrastructure.Services;

public class PostService : IPostService
{
    public Task<IEnumerable<PostListDto>> GetPostsAsync(int page, int pageSize, string sortBy, bool descending)
        => throw new NotImplementedException();

    public Task<PostDto> GetPostByIdAsync(int id)
        => throw new NotImplementedException();

    public Task<PostDto> GetPostBySlugAsync(string slug)
        => throw new NotImplementedException();

    public Task<IEnumerable<PostListDto>> GetPostsByCategoryAsync(int categoryId, int page, int pageSize)
        => throw new NotImplementedException();

    public Task<PostDto> CreatePostAsync(CreatePostRequest request)
        => throw new NotImplementedException();

    public Task<PostDto> UpdatePostAsync(int id, UpdatePostRequest request)
        => throw new NotImplementedException();

    public Task DeletePostAsync(int id)
        => throw new NotImplementedException();

    public Task VotePostAsync(int id, VoteType voteType)
        => throw new NotImplementedException();

    public Task<IEnumerable<CommentDto>> GetPostCommentsAsync(int postId, int page, int pageSize)
        => throw new NotImplementedException();
}
