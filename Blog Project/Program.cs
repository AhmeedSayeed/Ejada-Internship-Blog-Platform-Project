
using Application.Interfaces;
using Blog_Project.Application.Filters;
using Blog_Project.Application.Interfaces;
using Blog_Project.Application.Mappings;
using Blog_Project.Application.Services;
using Blog_Project.Data;
using Blog_Project.Domain.Models;
using Blog_Project.Extensions;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Blog_Project
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });


            // DbContext Configuration
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<AppDbContext>());

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false; //
                options.User.AllowedUserNameCharacters = string.Empty;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),

                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAutoMapper(config =>
            {
                config.AddProfile<UserProfile>();
                config.AddProfile<PostProfile>();
            });

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserProfileService, UserProfileService>();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await app.SeedRolesAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
