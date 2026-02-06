using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class BlogControllerTests
{
    [Fact]
    public void GetAllBlogs_ReturnsOkResult_WithListOfBlogs()
    {
        // Arrange
        var mockService = new Mock<IBlogService>();
        mockService.Setup(s => s.GetBlogs())
                   .Returns(new List<string> { "Blog1", "Blog2" });

        var controller = new BlogController(mockService.Object);

        // Act
        var result = controller.GetAllBlogs();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var blogs = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
        Assert.Equal(2, ((List<string>)blogs).Count);
    }
}
