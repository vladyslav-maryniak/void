using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using System;
using Void.BLL.Services;
using Void.BLL.Services.Abstractions;
using Void.Shared.Options;
using Void.WebAPI.Middlewares;

namespace Void.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration
                .GetSection(CoinGeckoOptions.Key)
                .Get<CoinGeckoOptions>();

            services
                .AddHttpClient<ICryptoDataProvider, CoinGeckoProvider>(client =>
                {
                    client.BaseAddress = new Uri($"{options.Scheme}://{options.Host}");
                })
                .AddTransientHttpErrorPolicy(policy =>
                {
                    return policy.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                        TimeSpan.FromSeconds(options.Policy.WaitAndRetry.MedianFirstRetryDelay),
                        options.Policy.WaitAndRetry.RetryCount));
                })
                .AddTransientHttpErrorPolicy(policy =>
                {
                    return policy.AdvancedCircuitBreakerAsync(
                        options.Policy.AdvancedCircuitBreaker.FailureThreshold,
                        TimeSpan.FromSeconds(options.Policy.AdvancedCircuitBreaker.SamplingDuration),
                        options.Policy.AdvancedCircuitBreaker.MinimumThroughput,
                        TimeSpan.FromSeconds(options.Policy.AdvancedCircuitBreaker.DurationOfBreak));
                });
        }

        public static void AddCustomMiddlewares(this IServiceCollection services)
        {
            services.AddTransient<PolicyExceptionHandlingMiddleware>();
        }
    }
}
