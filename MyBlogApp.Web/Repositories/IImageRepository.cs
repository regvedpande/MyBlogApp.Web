namespace MyBlogApp.Web.Repositories
{
    public class IImageRepository
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
