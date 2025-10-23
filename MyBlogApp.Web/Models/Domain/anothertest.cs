using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MyApp.Data;
using MyApp.Models;
using MyApp.Repositories;
using System.Linq;

namespace MyApp.Tests
{
    public class EmployeeRepositoryTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetWithAtLeastTwoYearsAsync_ReturnsOnlyThoseWith2OrMoreYears()
        {
            // Arrange
            using var ctx = CreateInMemoryContext();
            var repo = new EmployeeRepository(ctx);

            var now = DateTime.UtcNow.Date;
            var e1 = new Employee { Name = "Alice", DateOfJoining = now.AddYears(-3), Position = "Dev" }; // 3 years
            var e2 = new Employee { Name = "Bob", DateOfJoining = now.AddYears(-2).AddDays(-1), Position = "QA" }; // > 2 years
            var e3 = new Employee { Name = "Carol", DateOfJoining = now.AddYears(-1), Position = "Dev" }; // 1 year

            ctx.Employees.AddRange(e1, e2, e3);
            await ctx.SaveChangesAsync();

            // Act
            var result = await repo.GetWithAtLeastTwoYearsAsync();

            // Assert
            Assert.Contains(result, r => r.Name == "Alice");
            Assert.Contains(result, r => r.Name == "Bob");
            Assert.DoesNotContain(result, r => r.Name == "Carol");
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task YearsOfExperience_ComputedProperty_Works()
        {
            var e = new Employee { DateOfJoining = DateTime.UtcNow.AddYears(-5).AddDays(-1) };
            Assert.True(e.YearsOfExperience >= 5);
        }
    }
}