using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyBlogApp.Web.Data;
using MyBlogApp.Web.Models.Domain;
using MyBlogApp.Web.Repositories;
using MyBlogApp.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register IHttpContextAccessor so layout can read claims and HttpContext
builder.Services.AddHttpContextAccessor();

// Database - make sure your connection string exists in appsettings.json
builder.Services.AddDbContext<BloggieDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString"));
});

// Repositories
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Email service
builder.Services.AddTransient<IEmailService, EmailService>();

// Authentication - cookie based
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.Cookie.Name = "MyBlogAuth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

// Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Optional: migrate DB and seed minimal content for dev
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<BloggieDbContext>();
        db.Database.Migrate();

        // Seed tags & sample posts if none exist (safe dev helper)
        if (!db.Tags.Any())
        {
            var tags = new[]
            {
                new Tag { Id = Guid.NewGuid(), Name = "dotnet", DisplayName = ".NET" },
                new Tag { Id = Guid.NewGuid(), Name = "microservices", DisplayName = "Microservices" },
                new Tag { Id = Guid.NewGuid(), Name = "devops", DisplayName = "DevOps" },
                new Tag { Id = Guid.NewGuid(), Name = "cloud", DisplayName = "Cloud" },
                new Tag { Id = Guid.NewGuid(), Name = "security", DisplayName = "Security" },
                new Tag { Id = Guid.NewGuid(), Name = "architecture", DisplayName = "Architecture" }
            };
            db.Tags.AddRange(tags);
            db.SaveChanges();

            var now = DateTime.UtcNow;
            var samplePosts = new[]
            {
                new BlogPost {
                    Id = Guid.NewGuid(),
                    Heading = "Microservices",
                    PageTitle = "Microservices guide",
                    ShortDescription = "A short primer on microservices architecture.",
                    Content = "<p>This is a seeded post about microservices.</p>",
                    FeaturedImageUrl = "https://picsum.photos/seed/microservices/1200/600",
                    UrlHandle = "microservices",
                    PublishedDate = now.AddDays(-30),
                    Author = "Seed Admin",
                    Visible = true,
                    Tags = new List<Tag> { tags[1], tags[5] }
                },
                new BlogPost {
                    Id = Guid.NewGuid(),
                    Heading = "Getting Started with .NET 8",
                    PageTitle = ".NET 8 quick start",
                    ShortDescription = "How to get productive quickly with .NET 8.",
                    Content = "<p>Welcome to a quickstart for .NET 8.</p>",
                    FeaturedImageUrl = "https://picsum.photos/seed/dotnet/1200/600",
                    UrlHandle = "net-8-quickstart",
                    PublishedDate = now.AddDays(-25),
                    Author = "Seed Admin",
                    Visible = true,
                    Tags = new List<Tag> { tags[0] }
                },
                new BlogPost {
                    Id = Guid.NewGuid(),
                    Heading = "Cloud Costs Optimization",
                    PageTitle = "Optimize Cloud Costs",
                    ShortDescription = "Tips to reduce cloud bills.",
                    Content = "<p>Cost-saving strategies for cloud deployments.</p>",
                    FeaturedImageUrl = "https://picsum.photos/seed/cloud/1200/600",
                    UrlHandle = "cloud-costs",
                    PublishedDate = now.AddDays(-21),
                    Author = "Seed Admin",
                    Visible = true,
                    Tags = new List<Tag> { tags[3] }
                },
                new BlogPost {
                    Id = Guid.NewGuid(),
                    Heading = "DevOps CI/CD Pipelines",
                    PageTitle = "CI/CD Pipelines",
                    ShortDescription = "Build automated delivery pipelines.",
                    Content = "<p>CI/CD best practices and examples.</p>",
                    FeaturedImageUrl = "https://picsum.photos/seed/devops/1200/600",
                    UrlHandle = "cicd-pipelines",
                    PublishedDate = now.AddDays(-15),
                    Author = "Seed Admin",
                    Visible = true,
                    Tags = new List<Tag> { tags[2] }
                },
                new BlogPost {
                    Id = Guid.NewGuid(),
                    Heading = "Application Security Basics",
                    PageTitle = "Security Basics",
                    ShortDescription = "Secure web apps from common threats.",
                    Content = "<p>OWASP-informed recommendations and controls.</p>",
                    FeaturedImageUrl = "https://picsum.photos/seed/security/1200/600",
                    UrlHandle = "app-security",
                    PublishedDate = now.AddDays(-10),
                    Author = "Seed Admin",
                    Visible = true,
                    Tags = new List<Tag> { tags[4] }
                },
                new BlogPost {
                    Id = Guid.NewGuid(),
                    Heading = "Designing for Scale",
                    PageTitle = "Design for Scale",
                    ShortDescription = "Architect systems that scale gracefully.",
                    Content = "<p>Patterns for high-scale systems.</p>",
                    FeaturedImageUrl = "https://picsum.photos/seed/scale/1200/600",
                    UrlHandle = "design-scale",
                    PublishedDate = now.AddDays(-5),
                    Author = "Seed Admin",
                    Visible = true,
                    Tags = new List<Tag> { tags[5], tags[1] }
                }
            };

            db.BlogPosts.AddRange(samplePosts);
            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        // In dev you can log or inspect the exception. Avoid crashing in production setup.
        var logger = services.GetService<ILogger<Program>>();
        logger?.LogError(ex, "Error applying migrations or seeding DB");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
