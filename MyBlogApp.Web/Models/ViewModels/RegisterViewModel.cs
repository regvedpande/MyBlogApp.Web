using System.ComponentModel.DataAnnotations;

namespace MyBlogApp.Web.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [StringLength(200)]
        public string DisplayName { get; set; } = string.Empty;
    }
}
