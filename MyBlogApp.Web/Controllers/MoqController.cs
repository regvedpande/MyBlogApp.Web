using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using YourNamespace.Controllers;  // Replace with your actual namespace
using YourNamespace.Services;   // Replace with your actual namespace
using YourNamespace.Models;     // Replace with your actual namespace

namespace YourNamespace.Tests
{
    public class BlogControllerTests
    {
        [Fact]
        public void GetBlogs_ReturnsOkResult_WithAListOfBlogs()
        {
            // Arrange
            var mockBlogService = new Mock<IBlogService>();
            mockBlogService
                .Setup(service => service.GetAllBlogs())
                .Returns(new List<Blog>
                {
                    new Blog { Id = 1, Title = "Test Blog 1", Content = "Content for Blog 1" },
                    new Blog { Id = 2, Title = "Test Blog 2", Content = "Content for Blog 2" }
                });

            var controller = new BlogController(mockBlogService.Object);

            // Act
            var result = controller.GetBlogs();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var blogs = Assert.IsAssignableFrom<IEnumerable<Blog>>(okResult.Value);
            Assert.Equal(2, blogs.Count());
        }

        [Fact]
        public void GetBlogById_ReturnsNotFound_WhenBlogDoesNotExist()
        {
            // Arrange
            var mockBlogService = new Mock<IBlogService>();
            mockBlogService
                .Setup(service => service.GetBlogById(1))
                .Returns((Blog)null); // Simulate no blog found

            var controller = new BlogController(mockBlogService.Object);

            // Act
            var result = controller.GetBlogById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetBlogById_ReturnsOkResult_WithBlogItem()
        {
            // Arrange
            var mockBlogService = new Mock<IBlogService>();
            var blog = new Blog { Id = 1, Title = "Test Blog", Content = "Content for Test Blog" };
            mockBlogService
                .Setup(service => service.GetBlogById(1))
                .Returns(blog);

            var controller = new BlogController(mockBlogService.Object);

            // Act
            var result = controller.GetBlogById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBlog = Assert.IsType<Blog>(okResult.Value);
            Assert.Equal(blog.Id, returnedBlog.Id);
        }
    }
}