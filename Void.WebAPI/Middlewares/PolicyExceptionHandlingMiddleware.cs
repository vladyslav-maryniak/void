using Microsoft.AspNetCore.Http;
using Polly.CircuitBreaker;
using System;
using System.Net;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;

namespace Void.WebAPI.Middlewares
{
    public class PolicyExceptionHandlingMiddleware : IMiddleware
    {
        private readonly INotifier notifier;

        public PolicyExceptionHandlingMiddleware(INotifier notifier)
        {
            this.notifier = notifier;
        }

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
            catch (Exception e)
            {
                await notifier.NotifyAsync(e.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new { Error = "Middleware Exception\n" + e.Message });
            }
        }
    }
}
