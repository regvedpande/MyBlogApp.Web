using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyBlogApp.Web.Models.ViewModels;
using MyBlogApp.Web.Repositories;
using System.Threading.Tasks;

namespace MyBlogApp.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminBlogPostsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
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

        public async Task<IActionResult> Add(AddBlogPostsRequest addBlogPostsRequest)
        {
            return RedirectToAction("Add");                                                                 
        }
    }
}
