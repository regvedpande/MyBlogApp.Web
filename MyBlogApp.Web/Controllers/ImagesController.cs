using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyBlogApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index(IFormFile file)
        {
            
        }
    }
}
