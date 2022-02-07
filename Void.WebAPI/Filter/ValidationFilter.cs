using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using Void.WebAPI.Contracts;

namespace Void.WebAPI.Filter
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var modelStateErrors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage))
                    .ToArray();

                ValidationErrorResponse errorResponse = new();

                foreach (var error in modelStateErrors)
                {
                    foreach (var innerError in error.Value)
                    {
                        ValidationErrorModel model = new()
                        {
                            PropertyName = error.Key,
                            Message = innerError
                        };
                        errorResponse.Errors.Add(model);
                    }
                }

                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            await next();
        }
    }
}
