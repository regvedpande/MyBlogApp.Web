using MyBlogApp.Web.Models.Domain;

namespace MyBlogApp.Web.Models.ViewModels
{
    public class BlogDetailsViewModel
    {
        public BlogPost BlogPost { get; set; } = new();

        public IEnumerable<BlogPost> RelatedPosts { get; set; } = [];
    }
}
