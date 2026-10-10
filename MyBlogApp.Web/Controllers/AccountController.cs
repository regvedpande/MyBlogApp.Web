using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlogApp.Web.Models.Domain;
using MyBlogApp.Web.Models.ViewModels;
using MyBlogApp.Web.Repositories;
using MyBlogApp.Web.Services;

namespace MyBlogApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _emailService;

        public AccountController(
            IUserRepository userRepository,
            IWebHostEnvironment env,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
            _env = env;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existing = await _userRepository.GetByUserNameAsync(model.UserName);
            if (existing != null)
            {
                ModelState.AddModelError("", "User name already taken");
                return View(model);
            }

            // create user
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = model.UserName,
                Email = model.Email,
                DisplayName = string.IsNullOrWhiteSpace(model.DisplayName) ? model.UserName : model.DisplayName,
                ApiKey = Guid.NewGuid().ToString("N"),
                // give a default local avatar so DB doesn't receive NULL
                ProfileImageUrl = "/images/profiles/bb5cb604-4ba8-40fd-9cad-dcfa0eadf429.jpg"
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);


            await _userRepository.AddAsync(user);

            // Send welcome email (if SMTP configured). Don't block registration on failure.
            try
            {
                var subject = $"Welcome to MyBlog, {user.DisplayName ?? user.UserName}!";
                var html = $@"
                    <div style='font-family:Inter,system-ui,Arial,sans-serif; color:#111'>
                        <h2 style='color:#0f1724'>Welcome to MyBlog</h2>
                        <p>Hi {user.DisplayName ?? user.UserName},</p>
                        <p>Thanks for registering. Your account is ready.</p>
                        <p><strong>Your API key (keep private):</strong> <code style='background:#f3f4f6;padding:4px 8px;border-radius:6px'>{user.ApiKey}</code></p>
                        <p>If you need help, reply to this email.</p>
                        <hr />
                        <small style='color:#6b7280'>MyBlog Team</small>
                    </div>";
                await _emailService.SendEmailAsync(user.Email, subject, html);
            }
            catch
            {
                // log optionally; don't block registration if email fails
            }

            // sign-in user then redirect to profile
            await SignInUser(user);

            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userRepository.GetByUserNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View(model);
            }

            var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (verify == PasswordVerificationResult.Success)
            {
                await SignInUser(user);

                if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid credentials");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (User.Identity?.IsAuthenticated != true || string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                return RedirectToAction("Login");
            }

            var user = await _userRepository.GetByUserNameAsync(User.Identity.Name);
            if (user == null) return RedirectToAction("Login");

            var vm = new ProfileViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                ApiKey = user.ApiKey,
                CreatedAt = user.CreatedAt,
                ProfileImageUrl = user.ProfileImageUrl
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileEditViewModel vm)
        {
            if (User.Identity?.IsAuthenticated != true || string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                return Unauthorized();
            }

            var user = await _userRepository.GetByUserNameAsync(User.Identity.Name);
            if (user == null) return NotFound();

            // update allowed fields (currently only display name)
            if (!string.IsNullOrWhiteSpace(vm.DisplayName))
            {
                user.DisplayName = vm.DisplayName;
            }

            await _userRepository.UpdateAsync(user);

            // refresh cookie claims so UI reflects changes
            await SignInUser(user);

            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile profileImage)
        {
            if (User.Identity?.IsAuthenticated != true || string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                return Unauthorized();
            }

            var user = await _userRepository.GetByUserNameAsync(User.Identity.Name);
            if (user == null) return NotFound();

            if (profileImage != null && profileImage.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "images", "profiles");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                var ext = Path.GetExtension(profileImage.FileName).ToLowerInvariant();
                // Restrict allowed extensions (basic)
                var allowed = new[] { ".png", ".jpg", ".jpeg", ".gif", ".webp" };
                if (!allowed.Contains(ext))
                {
                    ModelState.AddModelError("", "Unsupported image format. Allowed: png, jpg, jpeg, gif, webp");
                    return RedirectToAction("Profile");
                }

                var fileName = $"{user.Id}{ext}";
                var filePath = Path.Combine(uploads, fileName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(fs);
                }

                // store relative url
                var relative = $"/images/profiles/{fileName}";
                user.ProfileImageUrl = relative;
                await _userRepository.UpdateAsync(user);

                // refresh cookie principal to include latest claims
                await SignInUser(user);
            }

            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> RegenerateApiKey()
        {
            if (User.Identity?.IsAuthenticated != true || string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                return Unauthorized();
            }

            var user = await _userRepository.GetByUserNameAsync(User.Identity.Name);
            if (user == null) return NotFound();

            user.ApiKey = Guid.NewGuid().ToString("N");
            await _userRepository.UpdateAsync(user);

            await SignInUser(user);

            return RedirectToAction("Profile");
        }

        // Helper: create cookie principal and include some useful claims
        private async Task SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("ApiKey", user.ApiKey ?? string.Empty)
            };

            if (!string.IsNullOrWhiteSpace(user.ProfileImageUrl))
            {
                // include profile image claim so layout can show avatar quickly
                claims.Add(new Claim("ProfileImage", user.ProfileImageUrl));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}
