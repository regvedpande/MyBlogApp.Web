namespace MyBlogApp.Web.Models.ViewModels
{
    public class ProfileViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string ApiKey { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
