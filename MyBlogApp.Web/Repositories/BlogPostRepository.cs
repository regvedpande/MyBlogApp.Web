using Bloggie.Web.Repositories;
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

        public Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
