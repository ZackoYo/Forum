using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Forum.Application.DTOs.Posts;
using Forum.Application.DTOs.Comments;
using Forum.Application.Services;
using Forum.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Forum.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostListDto>>> GetPosts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortBy = "created",
        [FromQuery] bool descending = true)
    {
        var posts = await _postService.GetPostsAsync(page, pageSize, sortBy, descending);
        return Ok(posts);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetPostById(int id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        return Ok(post);
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<PostDto>> GetPostBySlug(string slug)
    {
        var post = await _postService.GetPostBySlugAsync(slug);
        return Ok(post);
    }

    [HttpGet("category/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<PostListDto>>> GetPostsByCategory(
        int categoryId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var posts = await _postService.GetPostsByCategoryAsync(categoryId, page, pageSize);
        return Ok(posts);
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePost([FromBody] CreatePostRequest request)
    {
        var post = await _postService.CreatePostAsync(request);
        return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PostDto>> UpdatePost(int id, [FromBody] UpdatePostRequest request)
    {
        var post = await _postService.UpdatePostAsync(id, request);
        return Ok(post);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePost(int id)
{
        await _postService.DeletePostAsync(id);
        return NoContent();
    }
    {

    }
}
