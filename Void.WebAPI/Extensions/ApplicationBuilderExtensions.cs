using Microsoft.AspNetCore.Builder;
using Void.WebAPI.Middlewares;

namespace Void.WebAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UsePolicyExceptionHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<PolicyExceptionHandlingMiddleware>();
        }
    }
}
