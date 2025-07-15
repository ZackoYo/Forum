using Forum.Application.DTOs.Posts;
using Forum.Application.DTOs.Comments;
using Forum.Domain.Entities;

namespace Forum.Application.Services;

public interface IPostService
{
    Task<IEnumerable<PostListDto>> GetPostsAsync(int page, int pageSize, string sortBy, bool descending);
    Task<PostDto> GetPostByIdAsync(int id);
    Task<PostDto> GetPostBySlugAsync(string slug);
    Task<IEnumerable<PostListDto>> GetPostsByCategoryAsync(int categoryId, int page, int pageSize);
    Task<PostDto> CreatePostAsync(CreatePostRequest request);
    Task<PostDto> UpdatePostAsync(int id, UpdatePostRequest request);
    Task DeletePostAsync(int id);
    Task VotePostAsync(int id, VoteType voteType);
    Task<IEnumerable<CommentDto>> GetPostCommentsAsync(int postId, int page, int pageSize);
}