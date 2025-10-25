using Microsoft.AspNetCore.Mvc;
using MyBlogApp.Web.Models.Domain;

namespace MyBlogApp.Web.Models.ViewModels
{
    public class EditTagRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
