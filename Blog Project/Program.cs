
using Blog_Project.Data;
using Blog_Project.Domain.Models;
using Blog_Project.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog_Project
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();


            // DbContext Configuration
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false; //
            })  
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            ApplicationUser adminUser = new ApplicationUser();
           adminUser.
          

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await app.SeedRolesAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
