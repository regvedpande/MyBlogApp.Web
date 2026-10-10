namespace MyBlogApp.Web.Models.ViewModels
{
    public class EditTagRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}
