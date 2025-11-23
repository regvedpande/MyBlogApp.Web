using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Blog.Web.Controllers;
using Blog.Web.Models;
using Blog.Web.Services;

namespace Blog.Web.Tests
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
        public async Task Index_ReturnsViewResult_WithBlogList()
        {
            // Arrange
            var blogs = new List<Blog>
            {
                new Blog { Id = 1, Title = "Test Blog 1" },
                new Blog { Id = 2, Title = "Test Blog 2" }
            };

            _mockBlogService.Setup(service => service.GetAllBlogsAsync())
                .ReturnsAsync(blogs);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Blog>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithCorrectBlog()
        {
            // Arrange
            var blog = new Blog { Id = 1, Title = "Test Blog" };
            _mockBlogService.Setup(service => service.GetBlogByIdAsync(1))
                .ReturnsAsync(blog);

            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Blog>(viewResult.ViewData.Model);
            Assert.Equal("Test Blog", model.Title);
        }

        [Fact]
        public async Task Create_RedirectsToIndex_OnSuccess()
        {
            // Arrange
            var newBlog = new Blog { Title = "New Blog" };

            // Act
            var result = await _controller.Create(newBlog);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            _mockBlogService.Verify(s => s.AddBlogAsync(newBlog), Times.Once);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenBlogDoesNotExist()
        {
            // Arrange
            _mockBlogService.Setup(service => service.GetBlogByIdAsync(999))
                .ReturnsAsync((Blog)null);

            // Act
            var result = await _controller.Edit(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_DeletesBlogAndRedirectsToIndex()
        {
            // Arrange
            var blogToDelete = new Blog { Id = 1, Title = "Blog to Delete" };
            _mockBlogService.Setup(service => service.GetBlogByIdAsync(1))
                .ReturnsAsync(blogToDelete);

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            _mockBlogService.Verify(s => s.DeleteBlogAsync(1), Times.Once);
        }
    }
}