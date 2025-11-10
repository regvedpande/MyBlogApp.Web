using Bloggie.Web.Repositories;
using Microsoft.EntityFrameworkCore;
using MyBlogApp.Web.Data;
using MyBlogApp.Web.Models.Domain;

namespace MyBlogApp.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext BloggieDbContext;

        public BlogPostRepository(BloggieDbContext dbContext)
        {
            this.BloggieDbContext = dbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await BloggieDbContext.BlogPosts.AddAsync(blogPost);
            await BloggieDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await BloggieDbContext.BlogPosts.FindAsync(id);
            if (existingBlogPost != null)
            {
                BloggieDbContext.BlogPosts.Remove(existingBlogPost);
                await BloggieDbContext.SaveChangesAsync();
                return existingBlogPost;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await BloggieDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await BloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
           var existingBlog = await BloggieDbContext.BlogPosts.FirstOrDefaultAsync(x=>x.Id ==blogPost.Id);
            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.Content = blogPost.Content;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Author = blogPost.Author;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Tags = blogPost.Tags;

                await BloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }
    }
}
