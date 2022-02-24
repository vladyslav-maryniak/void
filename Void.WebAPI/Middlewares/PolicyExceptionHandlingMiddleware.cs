using Microsoft.AspNetCore.Http;
using Polly.CircuitBreaker;
using System.Net;
using System.Threading.Tasks;

namespace Void.WebAPI.Middlewares
{
    public class PolicyExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (BrokenCircuitException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                await context.Response.WriteAsJsonAsync(new { Error = "An error occurred connecting to external API" });
            }
        }
    }
}
