using Microsoft.AspNetCore.Mvc;
using MyBlogApp.Web.Models.ViewModels;
using MyBlogApp.Web.Repositories;
using MyBlogApp.Web.Services;
namespace MyBlogApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;
        public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository, ITagRepository tagRepository)
        {
            _logger = logger;
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Models.Domain.BlogPost> databasePosts = [];
            IEnumerable<Models.Domain.Tag> databaseTags = [];
            try
            {
                databasePosts = await blogPostRepository.GetAllAsync();
                databaseTags = await tagRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Database content is unavailable; showing the static blog catalog.");
            }

            var blogPosts = databasePosts
                .Concat(StaticBlogCatalog.Posts)
                .GroupBy(post => post.UrlHandle, StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .ToList();
            var tags = databaseTags
                .Concat(StaticBlogCatalog.Posts.SelectMany(post => post.Tags))
                .GroupBy(tag => tag.Name, StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .ToList();
            var model = new HomeViewModel
            {
                BlogPosts = blogPosts,
                Tags = tags
            };
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
