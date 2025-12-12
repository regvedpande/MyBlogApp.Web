public BlogController(BloggingContext db)
{
    _db = db;
}

// GET api/blog?page=1&pageSize=10&search=term
[HttpGet]
public async Task<ActionResult<IEnumerable<BlogListDto>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
{
    if (page <= 0) page = 1;
    if (pageSize <= 0 || pageSize > 100) pageSize = 10;

    var query = _db.Blogs.AsNoTracking();

    if (!string.IsNullOrWhiteSpace(search))
    {
        search = search.Trim();
        query = query.Where(b => EF.Functions.Like(b.Title, $"%{search}%") || EF.Functions.Like(b.Description!, $"%{search}%"));
    }

    var items = await query
        .OrderByDescending(b => b.CreatedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(b => new BlogListDto(
            b.Id,
            b.Title,
            b.Description,
            b.CreatedAt,
            b.Posts.Count))
        .ToListAsync();

    return Ok(items);
}

// GET api/blog/5
[HttpGet("{id:int}")]
public async Task<ActionResult<BlogDetailDto>> Get(int id)
{
    var blog = await _db.Blogs
        .AsNoTracking()
        .Where(b => b.Id == id)
        .Select(b => new BlogDetailDto(
            b.Id,
            b.Title,
            b.Description,
            b.CreatedAt,
            b.Posts.OrderByDescending(p => p.PublishedAt)
                   .Select(p => new PostDto(p.Id, p.Title, p.Content, p.PublishedAt))
        ))
        .FirstOrDefaultAsync();

    if (blog == null) return NotFound();
    return Ok(blog);
}

// POST api/blog
[HttpPost]
public async Task<ActionResult<BlogListDto>> Create([FromBody] CreateBlogDto dto)
{
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var blog = new Blog
    {
        Title = dto.Title,
        Description = dto.Description
    };

    _db.Blogs.Add(blog);
    await _db.SaveChangesAsync();

    var result = new BlogListDto(blog.Id, blog.Title, blog.Description, blog.CreatedAt, 0);
    return CreatedAtAction(nameof(Get), new { id = blog.Id }, result);
}

// PUT api/blog/5
[HttpPut("{id:int}")]
public async Task<IActionResult> Update(int id, [FromBody] UpdateBlogDto dto)
{
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var blog = await _db.Blogs.FindAsync(id);
    if (blog == null) return NotFound();

    blog.Title = dto.Title;
    blog.Description = dto.Description;
    // Optionally: track UpdatedAt

    await _db.SaveChangesAsync();
    return NoContent();
}

// DELETE api/blog/5
[HttpDelete("{id:int}")]
public async Task<IActionResult> Delete(int id)
{
    var blog = await _db.Blogs.Include(b => b.Posts).FirstOrDefaultAsync(b => b.Id == id);
    if (blog == null) return NotFound();

    // If you want cascade delete for posts, configure in EF Core or remove posts manually
    _db.Blogs.Remove(blog);
    await _db.SaveChangesAsync();
    return NoContent();
}

// POST api/blog/5/posts  -> create a post for a blog
[HttpPost("{id:int}/posts")]
public async Task<ActionResult<PostDto>> CreatePost(int id, [FromBody][Required] CreatePostDto dto)
{
    var blog = await _db.Blogs.FindAsync(id);
    if (blog == null) return NotFound();

    var post = new Post
    {
        Title = dto.Title,
        Content = dto.Content,
        PublishedAt = DateTime.UtcNow,
        BlogId = id
    };

    _db.Posts.Add(post);
    await _db.SaveChangesAsync();

    var result = new PostDto(post.Id, post.Title, post.Content, post.PublishedAt);
    return CreatedAtAction(nameof(Get), new { id }, result);
}