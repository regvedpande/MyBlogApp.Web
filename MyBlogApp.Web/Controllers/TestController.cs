using Microsoft.AspNetCore.Mvc;

namespace MyBlogApp.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Hello()
        {
            return Content("Hello from TestController");
        }
    }

}
