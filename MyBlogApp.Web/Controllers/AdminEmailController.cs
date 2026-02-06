using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogApp.Web.Models.ViewModels;
using MyBlogApp.Web.Repositories;
using MyBlogApp.Web.Services;

namespace MyBlogApp.Web.Controllers
{
    [Authorize]
    public class AdminEmailController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public AdminEmailController(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Compose()
        {
            return View(new MarketingEmailViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Compose(MarketingEmailViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var users = (await _userRepository.GetAllAsync())
                        .Where(u => !string.IsNullOrWhiteSpace(u.Email))
                        .ToList();

            int sent = 0;
            foreach (var user in users)
            {
                // simple personalization: replace placeholders
                var personalized = model.Body
                    .Replace("{{displayName}}", user.DisplayName ?? user.UserName)
                    .Replace("{{username}}", user.UserName);

                try
                {
                    await _emailService.SendEmailAsync(user.Email, model.Subject, personalized);
                    sent++;
                }
                catch
                {
                    // ignore single failures - you may want to log these
                }
            }

            ViewBag.Message = $"Attempted to send to {users.Count}. Successfully sent: {sent}.";
            return View(model);
        }
    }
}
