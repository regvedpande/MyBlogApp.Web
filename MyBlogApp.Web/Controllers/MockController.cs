[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlog(int id)
    {
        var blog = await _blogService.GetBlogByIdAsync(id);
        if (blog == null)
            return NotFound();

        return Ok(blog);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlog([FromBody] Blog blog)
    {
        var created = await _blogService.CreateBlogAsync(blog);
        return CreatedAtAction(nameof(GetBlog), new { id = created.Id }, created);
    }
}
