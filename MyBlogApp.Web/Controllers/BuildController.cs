// BlogControllerTests.cs
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;

public class BlogControllerTests
{
    [Fact]
    public void GetBlogById_ReturnsOkResult_WhenBlogExists()
    {
        // Arrange
        var mockService = new Mock<IBlogService>();
        mockService.Setup(s => s.GetBlog(1))
                   .Returns(new Blog { Id = 1, Title = "Test Blog" });

        var controller = new BlogController(mockService.Object);

        // Act
        var result = controller.GetBlogById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var blog = Assert.IsType<Blog>(okResult.Value);
        Assert.Equal(1, blog.Id);
        Assert.Equal("Test Blog", blog.Title);
    }

    [Fact]
    public void GetBlogById_ReturnsNotFound_WhenBlogDoesNotExist()
    {
        // Arrange
        var mockService = new Mock<IBlogService>();
        mockService.Setup(s => s.GetBlog(99)).Returns((Blog)null);

        var controller = new BlogController(mockService.Object);

        // Act
        var result = controller.GetBlogById(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
