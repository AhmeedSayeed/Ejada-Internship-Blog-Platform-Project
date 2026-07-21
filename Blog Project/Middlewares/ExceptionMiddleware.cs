using System.Net;
using System.Text.Json;

namespace Blog_Project.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = ex.Message switch
                {
                    "Post not found." => StatusCodes.Status404NotFound,
                    "Author not found." => StatusCodes.Status404NotFound,
                    "Category not found." => StatusCodes.Status404NotFound,

                    "You are not authorized to update this post." =>
                        StatusCodes.Status403Forbidden,

                    "You are not authorized to delete this post." =>
                        StatusCodes.Status403Forbidden,

                    _ => StatusCodes.Status500InternalServerError
                };

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
        }
    }
}