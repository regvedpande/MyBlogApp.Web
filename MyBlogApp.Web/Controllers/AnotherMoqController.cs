using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var blogs = _blogService.GetAll();
        return Ok(blogs);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var blog = _blogService.GetById(id);
        if (blog == null) return NotFound();
        return Ok(blog);
    }

    [HttpPost]
    public IActionResult Create(Blog blog)
    {
        _blogService.Create(blog);
        return CreatedAtAction(nameof(GetById), new { id = blog.Id }, blog);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _blogService.Delete(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
