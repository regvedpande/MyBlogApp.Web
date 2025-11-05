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

        public Task<BlogPost?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await BloggieDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await BloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
