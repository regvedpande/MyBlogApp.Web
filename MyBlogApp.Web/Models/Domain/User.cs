using System.ComponentModel.DataAnnotations;

namespace MyBlogApp.Web.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        // Hashed password
        public string PasswordHash { get; set; } = string.Empty;

        // Test API key
        public string ApiKey { get; set; } = string.Empty;

        // Display profile name
        public string DisplayName { get; set; } = string.Empty;

        // Optional profile image url (relative or absolute)
        public string ProfileImageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
