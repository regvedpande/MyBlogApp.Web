using Microsoft.AspNetCore.Mvc;
using MyBlogApp.Web.Repositories;
using MyBlogApp.Web.Services;
namespace MyBlogApp.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        public BlogsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blogPost = StaticBlogCatalog.Find(urlHandle);
            if (blogPost is null)
            {
                blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);
            }
            return View(blogPost);
        }
    }
}
