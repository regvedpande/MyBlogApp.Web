using Microsoft.AspNetCore.Mvc;
using MyBlogApp.Web.Models.Domain;
using MyBlogApp.Web.Models.ViewModels;
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
            if (string.IsNullOrWhiteSpace(urlHandle))
            {
                return NotFound();
            }

            var blogPost = StaticBlogCatalog.Find(urlHandle);
            if (blogPost is null)
            {
                blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);
            }

            if (blogPost is null || !blogPost.Visible)
            {
                return NotFound();
            }

            var model = new BlogDetailsViewModel
            {
                BlogPost = blogPost,
                RelatedPosts = await GetRelatedPostsAsync(blogPost)
            };

            return View(model);
        }

        private async Task<IEnumerable<BlogPost>> GetRelatedPostsAsync(BlogPost currentPost)
        {
            IEnumerable<BlogPost> databasePosts = [];

            try
            {
                databasePosts = await blogPostRepository.GetAllAsync();
            }
            catch
            {
                // Static posts still give readers useful onward paths when the database is unavailable.
            }

            var currentTagNames = (currentPost.Tags ?? [])
                .Select(tag => tag.Name)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return databasePosts
                .Concat(StaticBlogCatalog.Posts)
                .Where(post => post.Visible && !string.Equals(post.UrlHandle, currentPost.UrlHandle, StringComparison.OrdinalIgnoreCase))
                .GroupBy(post => post.UrlHandle, StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .Select(post => new
                {
                    Post = post,
                    MatchingTags = (post.Tags ?? []).Count(tag => !string.IsNullOrWhiteSpace(tag.Name) && currentTagNames.Contains(tag.Name))
                })
                .OrderByDescending(item => item.MatchingTags)
                .ThenByDescending(item => item.Post.PublishedDate)
                .Take(3)
                .Select(item => item.Post)
                .ToList();
        }
    }
}
