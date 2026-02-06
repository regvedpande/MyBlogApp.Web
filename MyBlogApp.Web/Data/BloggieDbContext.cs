using Microsoft.EntityFrameworkCore;
using MyBlogApp.Web.Models.Domain;

namespace MyBlogApp.Web.Data
{
    public class BloggieDbContext : DbContext
    {
        public BloggieDbContext(DbContextOptions<BloggieDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }

        // Users table for simple authentication
        public DbSet<User> Users { get; set; }
    }
}
