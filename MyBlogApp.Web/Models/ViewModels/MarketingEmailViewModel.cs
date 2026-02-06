using System.ComponentModel.DataAnnotations;

namespace MyBlogApp.Web.Models.ViewModels
{
    public class MarketingEmailViewModel
    {
        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
