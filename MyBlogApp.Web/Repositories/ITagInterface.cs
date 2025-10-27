using MyBlogApp.Web.Models.Domain;

namespace MyBlogApp.Web.Repositories
{
    public class ITagInterface
    {
        Task<IEnumerator<Tag>> GetAllAsync();
        Task<Tag> GetAsync(Guid id);
        Task AddAsync(Tag tag);
        Task UpdateAsync(Tag tag);
        Task DeleteAsync(Guid id);
    }
}
