using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyBlogApp.Web.Data;
using MyBlogApp.Web.Models.Domain;
using MyBlogApp.Web.Models.ViewModels;
using System.Linq.Expressions;

namespace MyBlogApp.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private BloggieDbContext bloggieDbContext;

        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(AddTagRequest addTagRequest)
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

            bloggieDbContext.Tags.Add(tag);
            bloggieDbContext.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public IActionResult List()
        {
            // use the bloggieDbContext to get all tags from the database
            var tags = bloggieDbContext.Tags.ToList();
            return View(tags);

        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            //var tag = bloggieDbContext.Tags.Find(id);
            //or
            var tag = bloggieDbContext.Tags.FirstOrDefault(x => x.Id == id);
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
        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            //We are converting it back into domain model
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
            var existingTag = bloggieDbContext.Tags.Find(tag.Id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                bloggieDbContext.SaveChanges();
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}
