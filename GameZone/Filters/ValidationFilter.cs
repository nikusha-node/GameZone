using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameZone.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null)
                    continue;

                var validatorType = typeof(IValidator<>)
                    .MakeGenericType(argument.GetType());

                var validator = _serviceProvider.GetService(validatorType);

                if (validator == null)
                    continue;

                var validationContext = new ValidationContext<object>(argument);

                var result = await ((IValidator)validator)
                    .ValidateAsync(validationContext);

                if (!result.IsValid)
                {
                    var errors = result.Errors
                        .Select(e => new
                        {
                            Property = e.PropertyName,
                            Message = e.ErrorMessage
                        });

                    context.Result = new BadRequestObjectResult(errors);
                    return;
                }
            }

            await next();
        }
    }
}