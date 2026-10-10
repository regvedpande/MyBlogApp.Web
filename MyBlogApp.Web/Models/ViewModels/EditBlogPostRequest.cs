using Microsoft.AspNetCore.Mvc.Rendering;
namespace MyBlogApp.Web.Models.ViewModels
{
    public class EditBlogPostRequest
    {
        public Guid Id { get; set; }
        public string Heading { get; set; } = string.Empty;
        public string PageTitle { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string FeaturedImageUrl { get; set; } = string.Empty;
        public string UrlHandle { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; } = string.Empty;
        public bool Visible { get; set; }
        public IEnumerable<SelectListItem> Tags { get; set; } = [];
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
