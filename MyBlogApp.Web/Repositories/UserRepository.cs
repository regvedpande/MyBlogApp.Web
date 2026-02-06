using Microsoft.EntityFrameworkCore;
using MyBlogApp.Web.Data;
using MyBlogApp.Web.Models.Domain;

namespace MyBlogApp.Web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BloggieDbContext _context;
        public UserRepository(BloggieDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByApiKeyAsync(string apiKey)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.ApiKey == apiKey);
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var existing = await _context.Users.FindAsync(user.Id);
            if (existing == null) return null;

            existing.DisplayName = user.DisplayName;
            existing.PasswordHash = user.PasswordHash;
            existing.ApiKey = user.ApiKey;
            existing.ProfileImageUrl = user.ProfileImageUrl;
            existing.Email = user.Email;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
