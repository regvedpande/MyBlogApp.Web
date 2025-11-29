using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class BlogTests
{
    private BlogDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique DB per test
            .Options;

        return new BlogDbContext(options);
    }

    [Fact]
    public void Can_Add_And_Retrieve_Blog()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var blog = new Blog
        {
            Title = "My First Blog",
            Content = "This is a test blog post."
        };

        // Act
        context.Blogs.Add(blog);
        context.SaveChanges();

        var retrievedBlog = context.Blogs.FirstOrDefault(b => b.Title == "My First Blog");

        // Assert
        Assert.NotNull(retrievedBlog);
        Assert.Equal("My First Blog", retrievedBlog.Title);
        Assert.Equal("This is a test blog post.", retrievedBlog.Content);
    }
}
