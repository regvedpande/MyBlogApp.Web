using MyBlogApp.Web.Models.Domain;

namespace MyBlogApp.Web.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User?> GetByUserNameAsync(string userName);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByApiKeyAsync(string apiKey);
        Task<User?> UpdateAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
    }
}
