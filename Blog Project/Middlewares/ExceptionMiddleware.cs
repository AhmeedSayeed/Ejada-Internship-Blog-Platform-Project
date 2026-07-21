using Blog_Project.Domain.Constants;
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
                    ErrorMessages.PostNotFound => StatusCodes.Status404NotFound,
                    ErrorMessages.AuthorNotFound => StatusCodes.Status404NotFound,
                    ErrorMessages.CategoryNotFound => StatusCodes.Status404NotFound,

                    ErrorMessages.UnauthorizedPostUpdate =>
                        StatusCodes.Status403Forbidden,

                    ErrorMessages.UnauthorizedPostDelete =>
                        StatusCodes.Status403Forbidden,
                        ErrorMessages.DraftOnly => StatusCodes.Status400BadRequest,
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