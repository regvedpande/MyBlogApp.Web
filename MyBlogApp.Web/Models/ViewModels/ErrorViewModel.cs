using System;

namespace MyBlogApp.Web.Models.ViewModels
{
    public class ErrorViewModel
    {
        // This property name is what the default template and generated Razor code expect
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
