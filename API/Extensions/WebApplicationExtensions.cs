using API.Infrastructure.Seed;
using System.Runtime.CompilerServices;

namespace API.Extensions
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
