using Forum.Application.DTOs.Comments;
using Forum.Domain.Entities;

namespace Forum.Application.Services;

public interface ICommentService
{
    Task<CommentDto> GetCommentByIdAsync(int id);
    Task<CommentDto> CreateCommentAsync(CreateCommentRequest request);
    Task<CommentDto> UpdateCommentAsync(int id, UpdateCommentRequest request);
    Task DeleteCommentAsync(int id);
    Task VoteCommentAsync(int id, VoteType voteType);
    Task<IEnumerable<CommentDto>> GetCommentRepliesAsync(int commentId, int page, int pageSize);
}