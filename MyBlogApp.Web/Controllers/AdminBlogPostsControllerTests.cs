using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyBlogApp.Web.Controllers;
using MyBlogApp.Web.Models.Domain;
using MyBlogApp.Web.Repositories;
using Xunit;

namespace MyBlogApp.Tests.Controllers
{
    public class AdminBlogPostsControllerTests
    {
        [Fact]
        public async Task List_ReturnsViewResult_WithBlogPosts()
        {
            // Arrange
            var mockTagRepo = new Mock<ITagRepository>();
            var mockBlogPostRepo = new Mock<IBlogPostRepository>();

            var fakeBlogPosts = new List<BlogPost>
            {
                new BlogPost { Id = Guid.NewGuid(), Heading = "Test Post 1" },
                new BlogPost { Id = Guid.NewGuid(), Heading = "Test Post 2" }
            };

            mockBlogPostRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(fakeBlogPosts);

            var controller = new AdminBlogPostsController(mockTagRepo.Object, mockBlogPostRepo.Object);

            // Act
            var result = await controller.List();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<BlogPost>>(viewResult.Model);
            Assert.Equal(2, ((List<BlogPost>)model).Count);
        }
    }
}