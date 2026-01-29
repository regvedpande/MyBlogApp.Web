using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MyApp.Controllers;
using MyApp.Models;
using MyApp.Services;

namespace MyApp.Tests.Controllers
{
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
        public void GetAll_ReturnsOkResult_WithListOfBlogs()
        {
            // Arrange
            var blogs = new List<Blog>
            {
                new Blog { Id = 1, Title = "First Post", Content = "Hello World" },
                new Blog { Id = 2, Title = "Second Post", Content = "Another Blog" }
            };
            _mockService.Setup(s => s.GetAll()).Returns(blogs);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBlogs = Assert.IsAssignableFrom<IEnumerable<Blog>>(okResult.Value);
            Assert.Equal(2, ((List<Blog>)returnBlogs).Count);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenBlogDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetById(99)).Returns((Blog)null);

            // Act
            var result = _controller.GetById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetById_ReturnsOkResult_WhenBlogExists()
        {
            // Arrange
            var blog = new Blog { Id = 1, Title = "Test Blog", Content = "Content" };
            _mockService.Setup(s => s.GetById(1)).Returns(blog);

            // Act
            var result = _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBlog = Assert.IsType<Blog>(okResult.Value);
            Assert.Equal("Test Blog", returnBlog.Title);
        }

        [Fact]
        public void Create_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newBlog = new Blog { Id = 3, Title = "New Blog", Content = "New Content" };
            _mockService.Setup(s => s.Create(newBlog)).Returns(newBlog);

            // Act
            var result = _controller.Create(newBlog);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnBlog = Assert.IsType<Blog>(createdAtActionResult.Value);
            Assert.Equal("New Blog", returnBlog.Title);
        }

        [Fact]
        public void Delete_ReturnsNoContent_WhenBlogExists()
        {
            // Arrange
            _mockService.Setup(s => s.Delete(1)).Returns(true);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenBlogDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.Delete(99)).Returns(false);

            // Act
            var result = _controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
