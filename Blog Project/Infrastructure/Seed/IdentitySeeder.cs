using Blog_Project.Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace Blog_Project.Infrastructure.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            foreach (var role in AppRoles.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }
        }
    }
}
