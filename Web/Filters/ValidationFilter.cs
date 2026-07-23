using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Filters
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
                        foreach (var error in validationResult.Errors)
                        {
                            context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                        }

                        var controller = context.Controller as Controller;

                        var viewResult = new ViewResult
                        {
                            ViewData = controller?.ViewData,
                            TempData = controller?.TempData
                        };

                        if (viewResult.ViewData != null)
                        {
                            viewResult.ViewData.Model = argument;
                        }

                        context.Result = viewResult;
                        return;
                    }
                }
            }

            // Proceed to the controller if the data is valid or if no validator was found
            await next();
        }
    }
}
