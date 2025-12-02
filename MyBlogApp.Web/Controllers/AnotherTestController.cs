using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using YourNamespace.Controllers;
using YourNamespace.Models;
using YourNamespace.Services;

namespace YourNamespace.Tests
{
    public class BlogControllerTests
    {
        private readonly Mock<IBlogService> _mockBlogService;
        private readonly BlogController _controller;

        public BlogControllerTests()
        {
            _mockBlogService = new Mock<IBlogService>();
            _controller = new BlogController(_mockBlogService.Object);
        }

        [Fact]
        public async Task GetBlogs_ReturnsOkResult_WithListOfBlogs()
        {
            // Arrange
            var blogs = new List<Blog>
            {
                new Blog { Id = 1, Title = "Blog 1", Content = "Content 1" },
                new Blog { Id = 2, Title = "Blog 2", Content = "Content 2" }
            };
            _mockBlogService.Setup(svc => svc.GetAllBlogsAsync()).ReturnsAsync(blogs);

            // Act
            var result = await _controller.GetBlogs();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Blog>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetBlogById_ReturnsNotFound_WhenBlogDoesNotExist()
        {
            // Arrange
            int blogId = 1;
            _mockBlogService.Setup(svc => svc.GetBlogByIdAsync(blogId)).ReturnsAsync((Blog)null);

            // Act
            var result = await _controller.GetBlogById(blogId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetBlogById_ReturnsOkResult_WithBlog()
        {
            // Arrange
            int blogId = 1;
            var blog = new Blog { Id = blogId, Title = "Blog 1", Content = "Content 1" };
            _mockBlogService.Setup(svc => svc.GetBlogByIdAsync(blogId)).ReturnsAsync(blog);

            // Act
            var result = await _controller.GetBlogById(blogId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Blog>(okResult.Value);
            Assert.Equal(blogId, returnValue.Id);
        }

        [Fact]
        public async Task CreateBlog_ReturnsCreatedResult_WhenBlogIsCreated()
        {
            // Arrange
            var newBlog = new Blog { Title = "New Blog", Content = "New Content" };
            _mockBlogService.Setup(svc => svc.CreateBlogAsync(newBlog)).ReturnsAsync(newBlog);

            // Act
            var result = await _controller.CreateBlog(newBlog);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Blog>(createdResult.Value);
            Assert.Equal("New Blog", returnValue.Title);
        }

        [Fact]
        public async Task UpdateBlog_ReturnsNoContentResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            int blogId = 1;
            var updatedBlog = new Blog { Id = blogId, Title = "Updated Blog", Content = "Updated Content" };
            _mockBlogService.Setup(svc => svc.UpdateBlogAsync(blogId, updatedBlog)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateBlog(blogId, updatedBlog);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateBlog_ReturnsNotFound_WhenBlogDoesNotExist()
        {
            // Arrange
            int blogId = 999;
            var updatedBlog = new Blog { Id = blogId, Title = "Updated Blog", Content = "Updated Content" };
            _mockBlogService.Setup(svc => svc.UpdateBlogAsync(blogId, updatedBlog)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateBlog(blogId, updatedBlog);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteBlog_ReturnsNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            int blogId = 1;
            _mockBlogService.Setup(svc => svc.DeleteBlogAsync(blogId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteBlog(blogId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBlog_ReturnsNotFound_WhenBlogDoesNotExist()
        {
            // Arrange
            int blogId = 999;
            _mockBlogService.Setup(svc => svc.DeleteBlogAsync(blogId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteBlog(blogId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}