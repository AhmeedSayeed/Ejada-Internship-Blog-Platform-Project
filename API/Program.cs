
using API.Application.Filters;
using API.Application.Interfaces;
using API.Application.Mapping;
using API.Application.Mappings;
using API.Application.Services;
using API.Data;
using API.Domain.Models;
using API.Extensions;
using API.Infrastructure.FileStrorage;
using API.Middlewares;
using API.Middlewares;
using Application.Interfaces;
using Blog_Project.Application.Interfaces;
using Blog_Project.Application.Mappings;
using Blog_Project.Application.Services;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace API
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

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return new BadRequestObjectResult(new ApiErrorResponse { Title = "Validation failed.", Errors = errors });
                };
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
                config.AddProfile<CommentProfile>();
                config.AddProfile<LikeProfile>();

            });

           

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserProfileService, UserProfileService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IPostImgService ,PostImgService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ILikeService , LikeService>() ;


            builder.Services.Configure<FileStorageSettings>(builder.Configuration.GetSection("FileStorageSettings"));
            builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your token}"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });

            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();
            await app.SeedRolesAsync();

            var mapper = app.Services.GetRequiredService<IMapper>();
            //mapper.ConfigurationProvider.AssertConfigurationIsValid();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            

            app.UseStaticFiles();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
