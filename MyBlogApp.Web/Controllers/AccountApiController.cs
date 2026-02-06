using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogApp.Web.Repositories;

namespace MyBlogApp.Web.Controllers.Api
{
    [Route("api/account")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public AccountApiController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("apikey")]
        [Authorize]
        public async Task<IActionResult> GetApiKey()
        {
            var user = await _userRepository.GetByUserNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            return Content(user.ApiKey);
        }
    }
}
