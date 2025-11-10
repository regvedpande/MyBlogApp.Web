using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyBlogApp.Web.Models.Domain;
using MyBlogApp.Web.Models.ViewModels;
using MyBlogApp.Web.Repositories;
using System.Threading.Tasks;

namespace MyBlogApp.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository,IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult List()
        {
            var blogPosts = blogPostRepository.GetAllAsync();
            return View(blogPosts);
        }

        public async Task<IActionResult> Add()
        {
            //get all tags from the database
            var tags = await tagRepository.GetAllAsync();
            var model = new AddBlogPostsRequest
            {
                Tags = tags.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostsRequest addBlogPostsRequest)
        {
            //Map the ViewModel to Domain Model
            var blogPost = new BlogPost
            {
                Heading = addBlogPostsRequest.Heading,
                PageTitle = addBlogPostsRequest.PageTitle,
                Content = addBlogPostsRequest.Content,
                ShortDescription = addBlogPostsRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostsRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostsRequest.UrlHandle,
                PublishedDate = addBlogPostsRequest.PublishedDate,
                Author = addBlogPostsRequest.Author,
                Visible = addBlogPostsRequest.Visible
            };
            
            // Map Tags from selected Tags
            var selectedTags = new List<Tag>();

            foreach (var selectedTagId in addBlogPostsRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
                if(existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            //Mapping tags back to domain model
            blogPost.Tags = selectedTags;   

            await blogPostRepository.AddAsync(blogPost);
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await blogPostRepository.GetAsync(id);
            if(blogPost == null)
            {
                return View("NotFound");
            }
            var tags = await tagRepository.GetAllAsync();
            var model = new EditBlogPostRequest
            {
                Id = blogPost.Id,
                Heading = blogPost.Heading,
                PageTitle = blogPost.PageTitle,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                Visible = blogPost.Visible,
                Tags = tags.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }),
                SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            // Map ViewModel to Domain Model
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Author = editBlogPostRequest.Author,
                Visible = editBlogPostRequest.Visible
            };

            //Map Tags into Domain Model
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTagId, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);
                    if ( foundTag!= null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }
            blogPostDomainModel.Tags = selectedTags;

            //Submit Information to Repository to Update
            var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);
            if (updatedBlog != null)
            {
                return RedirectToAction("Edit");
            }
            return RedirectToAction("Index");
        }
    }
}
