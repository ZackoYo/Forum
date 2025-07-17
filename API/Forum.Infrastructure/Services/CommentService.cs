using Forum.Application.DTOs.Comments;
using Forum.Application.Services;
using Forum.Domain.Entities;

namespace Forum.Infrastructure.Services;

public class CommentService : ICommentService
{
    public Task<CommentDto> GetCommentByIdAsync(int id)
        => throw new NotImplementedException();

    public Task<CommentDto> CreateCommentAsync(CreateCommentRequest request)
        => throw new NotImplementedException();

    public Task<CommentDto> UpdateCommentAsync(int id, UpdateCommentRequest request)
        => throw new NotImplementedException();

    public Task DeleteCommentAsync(int id)
        => throw new NotImplementedException();

    public Task VoteCommentAsync(int id, VoteType voteType)
        => throw new NotImplementedException();

    public Task<IEnumerable<CommentDto>> GetCommentRepliesAsync(int commentId, int page, int pageSize)
        => throw new NotImplementedException();
}
