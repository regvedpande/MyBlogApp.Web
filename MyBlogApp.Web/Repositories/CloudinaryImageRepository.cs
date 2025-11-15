namespace MyBlogApp.Web.Repositories
{
    public class CloudinaryImageRepository : IImageRepository
    {
        public Task<string> UploadAsync(IFormFile file)
        {
            // Implementation for uploading image to Cloudinary
            throw new NotImplementedException();
        }
    }
}
