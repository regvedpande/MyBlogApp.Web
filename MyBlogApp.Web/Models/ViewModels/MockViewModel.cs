using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApplication.Models
{
    public class Blog
    {
        [Key] // This attribute marks the property as a primary key
        public int BlogId { get; set; }

        [Required] // Ensures this property is not null or empty
        [MaxLength(100)] // Defines the maximum length of the Title
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required] // Ensures a value must be provided
        [DataType(DataType.DateTime)] // Specifies the type to be used
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; } // Nullable for cases when no updates are made

        // You can include additional fields if needed
        [MaxLength(50)]
        public string Author { get; set; }
    }
}