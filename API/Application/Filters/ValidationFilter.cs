using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Application.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Iterate through all arguments passed to the controller action
            foreach (var argument in context.ActionArguments.Values.Where(v => v != null))
            {
                var argumentType = argument?.GetType();

                // Create the expected IValidator<T> type dynamically based on the argument
                var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);

                // Attempt to resolve the validator from the dependency injection container
                var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

                if (validator != null)
                {
                    var validationContext = new ValidationContext<object>(argument);
                    var validationResult = await validator.ValidateAsync(validationContext);

                    if (!validationResult.IsValid)
                    {
                        // Short-circuit the pipeline immediately. The controller will not execute.
                        var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                        context.Result = new BadRequestObjectResult(new ApiErrorResponse
                        {
                            Title = "Validation failed.",
                            Errors = errors
                        });
                        return;
                    }
                }
            }

            // Proceed to the controller if the data is valid or if no validator was found
            await next();
        }
    }
}
