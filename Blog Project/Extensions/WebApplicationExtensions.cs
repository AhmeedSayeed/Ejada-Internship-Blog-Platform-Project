using Blog_Project.Infrastructure.Seed;
using System.Runtime.CompilerServices;

namespace Blog_Project.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task SeedRolesAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            await IdentitySeeder.SeedRolesAsync(scope.ServiceProvider);
        }

    }
}
