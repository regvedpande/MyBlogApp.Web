using System.Diagnostics;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using MyBlogApp.Web.Models;
using MyBlogApp.Web.Repositories;

namespace MyBlogApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogPostRepository blogPostRepository;

        public HomeController(ILogger<HomeController> logger, BlogPostRepository blogPostRepository)
        {
            _logger = logger;
            this.blogPostRepository = blogPostRepository;
        }

        public IActionResult Index()
        {
            var blogPosts = blogPostRepository.GetAllAsync();
            return View(blogPosts);
        }

        public IActionResult Privacy()
        {
            var posts = blogPostRepository.GetAllAsync();
            return View(posts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
