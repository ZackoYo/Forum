using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Forum.Application.DTOs.Categories;
using Forum.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace Forum.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CategoryDto>> GetCategories()
    {
        var categories = _categoryService.GetAllCategories<CategoryDto>();
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        return Ok(category);
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<CategoryDto>> GetCategoryBySlug(string slug)
    {
        var category = await _categoryService.GetCategoryBySlugAsync(slug);
        return Ok(category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        var category = await _categoryService.CreateCategoryAsync(request);
        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
    }
}