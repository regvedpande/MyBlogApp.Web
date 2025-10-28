using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyBlogApp.Web.Data;
using MyBlogApp.Web.Models.Domain;
using MyBlogApp.Web.Models.ViewModels;
using MyBlogApp.Web.Repositories;
using System.Linq.Expressions;
namespace MyBlogApp.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;
        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            //var name = addTagRequest.Name;
            //var displayName = addTagRequest.DisplayName;
            //var name =  Request.Form["name"];
            //var displayName = Request.Form["displayName"];
            //Mapping AddTagRequest to Tag Domain Model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            await tagRepository.AddAsync(tag);
            return RedirectToAction("List");
        }
        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            // use the bloggieDbContext to get all tags from the database
            var tags = await tagRepository.GetAllAsync();
            return View(tags);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //var tag = bloggieDbContext.Tags.Find(id);
            //or
            var tag = await tagRepository.GetAsync(id);
            if (tag != null)
            {
                //We are converting it back into domain model
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            //We are converting it back into domain model
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
            var updatedTag = await tagRepository.UpdateAsync(tag);
            if (updatedTag != null)
            {
                return RedirectToAction("List");
            }
            else
            {
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await tagRepository.DeleteAsync(editTagRequest.Id);
            if (deletedTag != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}