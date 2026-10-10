namespace MyBlogApp.Web.Models.ViewModels
{
    public class ProfileViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string ProfileImageUrl { get; set; } = string.Empty;
    }
}
