using Microsoft.AspNetCore.Mvc;

namespace MyBlogApp.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        public IActionResult Add()
        {
            return View();
        }
    }
}
