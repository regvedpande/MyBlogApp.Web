using MyBlogApp.Web.Models.Domain;

namespace MyBlogApp.Web.Services;

public static class StaticBlogCatalog
{
    public static IReadOnlyList<BlogPost> Posts { get; } =
    [
        Create("building-calm-digital-products", "Building Calm Digital Products", "Small interface choices that reduce friction and help people stay focused.", "/images/blog/calm-products.svg", new DateTime(2026, 6, 24), "Design", "Product", """
            <p>Great products often feel quiet. They explain the next step, keep important actions within reach, and remove decisions that do not help someone move forward.</p>
            <h2>Start with the busy moments</h2><p>Look for screens where people pause, backtrack, or open another tab for help. Those moments reveal where the interface is asking for too much attention.</p>
            <blockquote>Clarity is not a visual style. It is the result of deciding what matters right now.</blockquote>
            <h2>Make progress visible</h2><p>Use clear status labels, helpful empty states, and immediate feedback after an action. A calm product earns trust one predictable interaction at a time.</p>
            """),
        Create("dotnet-api-that-ages-well", "A .NET API That Ages Well", "Practical boundaries, validation, and observability for maintainable ASP.NET Core APIs.", "/images/blog/dotnet-api.svg", new DateTime(2026, 6, 19), ".NET", "Architecture", """
            <p>A durable API is less about clever abstractions and more about making change inexpensive. Keep transport concerns at the edge and put business rules in code that can be tested without a server.</p>
            <h2>Prefer explicit contracts</h2><p>Request and response models protect the domain from accidental coupling. Validate at the boundary and return consistent problem details when input is invalid.</p>
            <h2>Design for diagnosis</h2><p>Structured logs, correlation IDs, health checks, and a small set of meaningful metrics turn production surprises into answerable questions.</p>
            """),
        Create("ship-smaller-safer", "Ship Smaller, Learn Faster", "A lightweight release approach for reducing risk without slowing down delivery.", "/images/blog/ship-smaller.svg", new DateTime(2026, 6, 15), "DevOps", "Delivery", """
            <p>Large releases hide assumptions. Smaller releases make feedback arrive while the context is still fresh and the cost of correction is low.</p>
            <h2>Decouple deploy from release</h2><p>Feature flags let teams merge and deploy incomplete work safely. Release to a small audience, watch the signals, then expand with confidence.</p>
            <h2>Keep the rollback boring</h2><p>Automate repeatable deployments, preserve backward compatibility during data changes, and rehearse recovery before it is urgent.</p>
            """),
        Create("accessible-by-default", "Accessible by Default", "Simple habits that make every interface more inclusive from the first commit.", "/images/blog/accessibility.svg", new DateTime(2026, 6, 8), "Accessibility", "Frontend", """
            <p>Accessibility works best as part of everyday engineering, not a final audit. Semantic HTML gives you a strong baseline before custom behavior is added.</p>
            <h2>Build a keyboard path</h2><p>Every interactive element should be reachable, understandable, and operable without a pointer. Visible focus is a feature, not decoration.</p>
            <h2>Test the real experience</h2><p>Automated checks catch common issues, but keyboard testing, zoom testing, and a screen reader pass reveal how the journey actually feels.</p>
            """),
        Create("useful-code-reviews", "Code Reviews People Enjoy", "How to make reviews faster, kinder, and more effective for the whole team.", "/images/blog/code-review.svg", new DateTime(2026, 5, 30), "Engineering", "Culture", """
            <p>A good review protects quality while sharing context. It focuses on the behavior of the code and leaves the author with a clear path forward.</p>
            <h2>Make changes easy to inspect</h2><p>Keep pull requests focused, explain the intent, and call out the areas where you want deeper feedback. Reviewers should not have to reconstruct the story.</p>
            <h2>Label the conversation</h2><p>Distinguish blockers from suggestions and questions. This small habit removes ambiguity and keeps taste from masquerading as correctness.</p>
            """),
        Create("measure-what-users-feel", "Measure What Users Feel", "Connect performance metrics to the moments that shape a user's experience.", "/images/blog/performance.svg", new DateTime(2026, 5, 22), "Performance", "Web", """
            <p>Performance is a product concern. A fast-looking dashboard can still feel slow if the action someone cares about gives no immediate response.</p>
            <h2>Measure journeys, not only pages</h2><p>Track navigation, interaction latency, failures, and completion time for important tasks. Segment the data so averages do not hide struggling users.</p>
            <h2>Pair numbers with observation</h2><p>Metrics tell you where to look. Session feedback, support themes, and usability tests help explain why the numbers changed.</p>
            """)
    ];

    public static BlogPost? Find(string? urlHandle) =>
        Posts.FirstOrDefault(post => string.Equals(post.UrlHandle, urlHandle, StringComparison.OrdinalIgnoreCase));

    private static BlogPost Create(string handle, string heading, string description, string image, DateTime published, string primaryTag, string secondaryTag, string content) =>
        new()
        {
            Id = Guid.Parse(CreateStableId(handle)), Heading = heading, PageTitle = heading,
            ShortDescription = description, FeaturedImageUrl = image, UrlHandle = handle,
            PublishedDate = DateTime.SpecifyKind(published, DateTimeKind.Utc), Author = "MyBlog Editorial",
            Visible = true, Content = content,
            Tags = [CreateTag(primaryTag), CreateTag(secondaryTag)]
        };

    private static Tag CreateTag(string name) => new() { Id = Guid.Empty, Name = name.ToLowerInvariant(), DisplayName = name, BlogPosts = [] };

    private static string CreateStableId(string value)
    {
        var bytes = System.Security.Cryptography.MD5.HashData(System.Text.Encoding.UTF8.GetBytes(value));
        return new Guid(bytes).ToString();
    }
}
