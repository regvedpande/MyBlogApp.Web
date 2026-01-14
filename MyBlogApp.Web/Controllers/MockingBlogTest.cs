using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BlogApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetBlogs()
        {
            return Ok(new List<string> { "Blog1", "Blog2" });
        }

        [HttpGet("{id}")]
        public ActionResult<string> GetBlogById(int id)
        {
            if (id <= 0) return NotFound($"Blog with ID {id} not found.");
            return Ok($"Blog{id}");
        }

        [HttpPost]
        public ActionResult<string> PostBlog([FromBody] string blog)
        {
            if (string.IsNullOrEmpty(blog)) return BadRequest("Invalid blog content.");
            return CreatedAtAction(nameof(GetBlogById), new { id = 1 }, blog);
        }
    }
}