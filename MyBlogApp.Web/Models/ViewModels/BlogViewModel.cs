using System;
using System.ComponentModel.DataAnnotations;

namespace YourAppNamespace.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        [StringLength(50)]
        public string Author { get; set; } = string.Empty;
    }
}
