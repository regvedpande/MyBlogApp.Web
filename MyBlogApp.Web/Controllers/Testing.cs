using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using YourNamespace.Controllers;
using YourNamespace.Models;
using YourNamespace.Services;

public class BlogControllerTests
{
    private readonly Mock<IBlogService> _blogServiceMock;
    private readonly BlogController _controller;

    public BlogControllerTests()
    {
        _blogServiceMock = new Mock<IBlogService>();
        _controller = new BlogController(_blogServiceMock.Object);
    }

    [Fact]
    public void GetPostById_ReturnsOk_WithPost()
    {
        // Arrange
        var post = new BlogPost { Id = 1, Title = "Hello World", Content = "My first blog post" };
        _blogServiceMock.Setup(s => s.GetPostById(1)).Returns(post);

        // Act
        var result = _controller.GetPostById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedPost = Assert.IsType<BlogPost>(okResult.Value);
        Assert.Equal(1, returnedPost.Id);
        Assert.Equal("Hello World", returnedPost.Title);
        Assert.Equal("My first blog post", returnedPost.Content);
    }
}
