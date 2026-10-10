using System.ComponentModel.DataAnnotations;
namespace MyBlogApp.Web.Models.ViewModels
{
    public class AddTagRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string DisplayName { get; set; } = string.Empty;
    }
}
