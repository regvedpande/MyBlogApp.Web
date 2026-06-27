
# MyBlog - Full-Stack .NET 8 Blogging Platform

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

**A complete, production-ready blogging platform showcasing modern ASP.NET Core development practices.**

Built to demonstrate full-stack .NET skills with real-world features like authentication, file uploads, email integration, and database management - all ready to run and explore locally.


## 👋 Welcome Visitors!

Thank you for checking out my project! This is a **fully functional blogging platform** I built to demonstrate my skills with the .NET ecosystem. Everything works out of the box - just follow the quick setup below to see it in action.

### What You'll See
- A polished, responsive blog homepage with **6 built-in editorial stories** and local artwork
- Story search, topic filters, featured content, and estimated reading times
- Complete user registration and authentication system
- Admin dashboard for managing posts and tags
- Rich text editor with image uploads
- Email functionality (welcome emails + marketing campaigns)
- Clean, maintainable code following best practices

**Time to explore:** ~10 minutes to set up, 5 minutes to see all features

---

## 🎯 What This Project Demonstrates

### Technical Skills Showcased

**Backend Development**
- ASP.NET Core 8 MVC architecture
- Entity Framework Core with Code-First migrations
- Repository pattern for data access
- Cookie-based authentication with secure password hashing (BCrypt)
- SMTP email integration

**Frontend Development**
- Razor Pages with clean, semantic HTML
- Responsive CSS design (mobile-friendly)
- Client-side story search and category filtering
- Accessible, locally hosted SVG artwork with no external image dependency
- WYSIWYG editor integration (Froala)
- Image upload and management

**Database & DevOps**
- SQL Server with EF Core migrations
- Automatic database seeding for demo data
- Connection string configuration
- Environment-based settings (appsettings.json)

**Real-World Features**
- User profiles with avatar uploads
- API key generation per user
- Marketing email campaigns with personalization
- Admin content management system
- File upload handling and storage

---

## 🚀 See It In Action (5-Minute Setup)

### What You'll Need
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (free)
- SQL Server LocalDB (comes with Visual Studio) or SQL Express (free)
- *(Optional)* [Mailtrap account](https://mailtrap.io/) for testing emails (free tier available)

### Setup Steps

```bash
# 1. Clone this repository
git clone https://github.com/yourusername/myblog.git
cd myblog

# 2. Update the database connection (appsettings.json)
# Use the default LocalDB connection or update to your SQL Server

# 3. Restore packages and create database
dotnet restore
dotnet ef database update

# 4. Run the application
dotnet run

# 5. Open your browser
# Visit: https://localhost:5001
```

**That's it!** The app will automatically seed 6 sample blog posts on first run.

### Optional: Enable Email Features

To test email functionality, add your SMTP settings to `appsettings.json`:

```json
"SmtpSettings": {
  "Host": "sandbox.smtp.mailtrap.io",
  "Port": 2525,
  "Username": "your_mailtrap_username",
  "Password": "your_mailtrap_password",
  "FromEmail": "noreply@myblog.com",
  "FromName": "MyBlog Team"
}
```

*(I recommend [Mailtrap](https://mailtrap.io/) - it's free and perfect for testing)*

---

## 🎬 Feature Walkthrough

### For Visitors (No Login Required)
1. **Homepage** - Browse 6 pre-loaded blog posts with featured images
2. **Post Detail** - Click any post to read the full content with rich formatting

### Try These Features (2 Minutes)
1. **Register** → Go to `/Account/Register` and create an account
   - You'll receive a welcome email (if SMTP is configured)
   - Automatic login after registration
   
2. **Profile** → Visit `/Account/Profile`
   - Upload a profile picture
   - Generate and view your API key
   - Edit your display name

3. **Create Content** → Go to `/Admin/Posts`
   - Create a new blog post with the visual editor
   - Upload images directly in the editor
   - Add tags and featured images
   - Publish and see it on the homepage

4. **Marketing Emails** → Visit `/Admin/Email`
   - Compose an HTML email
   - Send to all registered users
   - Use personalization tokens like `{UserName}`

---

## 🏗️ Technical Architecture

### How It Works

```
Browser Request
    ↓
MVC Controller (business logic)
    ↓
Repository Layer (data access)
    ↓
Entity Framework Core
    ↓
SQL Server Database
```

### Key Components

**Controllers**
- `HomeController` - Public blog views
- `AccountController` - Registration, login, profile management
- `AdminController` - Post/tag CRUD operations
- `AdminEmailController` - Marketing email campaigns

**Data Layer**
- Entity Framework Core with Code-First approach
- Repository pattern for clean separation of concerns
- Automatic migrations for database schema management

**Services**
- `EmailService` - SMTP integration for transactional and marketing emails
- File upload handling for avatars and blog images

**Security**
- BCrypt password hashing
- Cookie-based authentication
- Anti-forgery tokens on forms

---

## 📁 Project Structure

```
MyBlog/
├── Controllers/          # MVC controllers handling requests
├── Models/              # Database entities (User, Post, Tag)
├── Repositories/        # Data access layer
├── Services/            # Email service, business logic
├── Views/               # Razor UI templates
│   ├── Home/           # Public blog pages
│   ├── Account/        # Registration, login, profile
│   └── Admin/          # Content management
├── wwwroot/
│   ├── css/            # Styling
│   ├── images/         # Static assets
│   └── uploads/        # User-uploaded images
├── Migrations/          # EF Core database migrations
└── appsettings.json    # Configuration
```

---

## 💡 Design Decisions & Best Practices

### What I Built (And Why)

**Authentication Choice**
- Implemented custom cookie authentication with BCrypt hashing
- Shows understanding of auth fundamentals
- Production note: Would use ASP.NET Identity for larger projects

**Repository Pattern**
- Separates data access from business logic
- Makes code testable and maintainable
- Follows SOLID principles

**Database Seeding**
- Automatically populates demo content
- Makes the app "demo-ready" immediately
- Shows attention to presentation and user experience

**Configuration Management**
- Externalized settings (SMTP, connection strings)
- Environment-ready (dev/staging/prod configurations)
- Follows 12-factor app principles

**File Upload Strategy**
- Local storage (`wwwroot/uploads`) for development
- Easily swappable to cloud storage (Azure Blob, AWS S3)
- Production note: Would implement cloud storage for scalability

---

## 🔐 Security Considerations

**Current Implementation**
- ✅ Password hashing with BCrypt
- ✅ Anti-forgery tokens on forms
- ✅ Secure cookie authentication
- ✅ Parameterized SQL queries (EF Core)

**Production Recommendations**
- Add HTML sanitization for blog content (currently uses `@Html.Raw()`)
- Implement role-based authorization (admin flag)
- Add rate limiting for API endpoints
- Enable HTTPS enforcement
- Implement CSRF protection on all state-changing operations

---

## 🎓 What I Learned Building This

- Deep dive into ASP.NET Core MVC architecture
- Entity Framework Core migrations and seeding strategies
- SMTP integration and email templating
- File upload handling and validation
- Repository pattern implementation
- Cookie-based authentication from scratch
- Responsive UI design principles

---

## 🚀 Future Enhancements

**On My Roadmap**
- [ ] Implement role-based admin system
- [ ] Add comment system for blog posts
- [ ] Background job processing for emails (Hangfire)
- [ ] Cloud storage integration (Azure Blob)
- [ ] Unit and integration tests
- [ ] API endpoints with JWT authentication
- [ ] Pagination for blog posts
- [ ] Search functionality
- [ ] Deploy to Azure App Service

---

## 📋 FAQ

**Q: Can I use this code?**  
A: Yes! This project is MIT licensed. Feel free to use it as a learning resource or starting point.

**Q: Why not use ASP.NET Identity?**  
A: I wanted to demonstrate understanding of authentication fundamentals. Identity is great for production, but building auth from scratch shows deeper knowledge.

**Q: Is this production-ready?**  
A: It's production-minded but would need enhancements (see Security Considerations) for a real production deployment.

**Q: How long did this take to build?**  
A: Approximately [X hours/days] including planning, implementation, and documentation.

---

## 📞 Let's Connect

I'm actively looking for opportunities to work with .NET and full-stack development.

**Regved Pande**  
📧 Email: regregd@outlook.com  
    Phone Number: +91 9049874883
    
**Questions about this project?** Feel free to open an issue or reach out directly!

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

<p align="center">
  <strong>Thank you for visiting! ⭐ Star this repo if you found it helpful</strong>
</p>

<p align="center">
  Built with ❤️ using .NET 8, Entity Framework Core, and ASP.NET Core MVC
</p>
```
