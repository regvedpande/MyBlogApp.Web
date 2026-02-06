using System.ComponentModel.DataAnnotations;

namespace MyBlogApp.Web.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }

        // Hashed password
        public string PasswordHash { get; set; }

        // Test API key
        public string ApiKey { get; set; }

        // Display profile name
        public string DisplayName { get; set; }

        // Optional profile image url (relative or absolute)
        public string ProfileImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
