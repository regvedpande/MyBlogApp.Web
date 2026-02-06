using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class BlogControllerTests
{
    private readonly Mock<IBlogService> _mockService;
    private readonly BlogController _controller;

    public BlogControllerTests()
    {
        _mockService = new Mock<IBlogService>();
        _controller = new BlogController(_mockService.Object);
    }

    [Fact]
    public async Task GetBlog_ReturnsOk_WhenBlogExists()
    {
        // Arrange
        var blog = new Blog { Id = 1, Title = "Test Blog" };
        _mockService.Setup(s => s.GetBlogByIdAsync(1)).ReturnsAsync(blog);

        // Act
        var result = await _controller.GetBlog(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Blog>(okResult.Value);
        Assert.Equal("Test Blog", returnValue.Title);
    }

    [Fact]
    public async Task GetBlog_ReturnsNotFound_WhenBlogDoesNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.GetBlogByIdAsync(99)).ReturnsAsync((Blog)null);

        // Act
        var result = await _controller.GetBlog(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateBlog_ReturnsCreatedAtAction_WhenValidBlog()
    {
        // Arrange
        var blog = new Blog { Id = 2, Title = "New Blog" };
        _mockService.Setup(s => s.CreateBlogAsync(It.IsAny<Blog>())).ReturnsAsync(blog);

        // Act
        var result = await _controller.CreateBlog(blog);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<Blog>(createdResult.Value);
        Assert.Equal(2, returnValue.Id);
    }
}
