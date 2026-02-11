using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourNamespace.Controllers;
using YourNamespace.Models;
using YourNamespace.Services;

namespace YourNamespace.Tests
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
        public async Task GetAll_ReturnsOkResult_WithListOfBlogs()
        {
            // Arrange
            var blogs = new List<Blog>
            {
                new Blog { Id = 1, Title = "First Post", Content = "Hello World" },
                new Blog { Id = 2, Title = "Second Post", Content = "Another blog" }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(blogs);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBlogs = Assert.IsType<List<Blog>>(okResult.Value);
            Assert.Equal(2, returnBlogs.Count);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenBlogDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Blog)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenBlogIsCreated()
        {
            // Arrange
            var newBlog = new Blog { Id = 3, Title = "New Blog", Content = "Content here" };
            _mockService.Setup(s => s.CreateAsync(It.IsAny<Blog>())).ReturnsAsync(newBlog);

            // Act
            var result = await _controller.Create(newBlog);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnBlog = Assert.IsType<Blog>(createdResult.Value);
            Assert.Equal("New Blog", returnBlog.Title);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenBlogIsDeleted()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
