using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YourProject.Controllers
{
    public class TestController : Controller
    {
        // GET: /Test/
        public ActionResult Index()
        {
            // Returns a simple view
            ViewBag.Message = "Hello from TestController!";
            return View();
        }

        // GET: /Test/Details/5
        public ActionResult Details(int id)
        {
            // Simulate fetching data
            var testData = new { Id = id, Name = "Sample Item " + id, Created = DateTime.Now };
            return Json(testData, JsonRequestBehavior.AllowGet);
        }

        // GET: /Test/Hello
        public string Hello()
        {
            return "Hello World from MVC TestController!";
        }

        // POST: /Test/Create
        [HttpPost]
        public ActionResult Create(string name)
        {
            // Simulate saving data
            ViewBag.Message = $"Item '{name}' created successfully!";
            return View("Index");
        }
    }
}
