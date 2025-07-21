using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Forum.Application.DTOs.Comments;
using Forum.Application.Services;
using Forum.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Forum.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetComment(int id)
    {
        var comment = await _commentService.GetCommentByIdAsync(id);
        return Ok(comment);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CreateCommentRequest request)
    {
        var comment = await _commentService.CreateCommentAsync(request);
        return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CommentDto>> UpdateComment(int id, [FromBody] UpdateCommentRequest request)
    {
        var comment = await _commentService.UpdateCommentAsync(id, request);
        return Ok(comment);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteComment(int id)
    {
        await _commentService.DeleteCommentAsync(id);
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id:int}/vote")]
    public async Task<ActionResult> VoteComment(int id, [FromBody] VoteType voteType)
    {
        await _commentService.VoteCommentAsync(id, voteType);
        return NoContent();
    }

    [HttpGet("{id:int}/replies")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentReplies(
        int id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var replies = await _commentService.GetCommentRepliesAsync(id, page, pageSize);
        return Ok(replies);
    }
}