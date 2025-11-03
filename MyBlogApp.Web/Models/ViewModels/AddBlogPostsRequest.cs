using Microsoft.AspNetCore.Mvc.Rendering;
using MyBlogApp.Web.Models.Domain;

namespace MyBlogApp.Web.Models.ViewModels
{
    public class AddBlogPostsRequest
    {
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }

        // Display available tags in a dropdown
        public IEnumerable<SelectListItem> Tags { get; set; }

        // To capture selected tag from the user
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
