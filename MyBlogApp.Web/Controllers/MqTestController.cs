using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MyApp.Controllers;
using MyApp.Models;
using MyApp.Services;

namespace MyApp.Tests
{
    public class BlogControllerTests
    {
        [Fact]
        public void GetAllBlogs_ReturnsOkResult_WithListOfBlogs()
        {
            // Arrange
            var mockService = new Mock<IBlogService>();
            mockService.Setup(service => service.GetAllBlogs())
                       .Returns(new List<Blog>
                       {
                           new Blog { Id = 1, Title = "First Blog", Content = "Hello World" },
                           new Blog { Id = 2, Title = "Second Blog", Content = "Testing Blog Controller" }
                       });

            var controller = new BlogController(mockService.Object);

            // Act
            var result = controller.GetAllBlogs();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBlogs = Assert.IsType<List<Blog>>(okResult.Value);
            Assert.Equal(2, returnBlogs.Count);
        }
    }
}
