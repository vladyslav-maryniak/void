using Discord.WebSocket;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Void.BLL.BackgroundServices;
using Void.BLL.Services;
using Void.BLL.Services.Abstractions;
using Void.DAL;
using Void.Shared.Options;
using Void.WebAPI.Filter;

namespace Void.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICoinService, CoinService>();
            services.AddTransient<ITickerService, TickerService>();
            services.AddTransient<IExchangeService, ExchangeService>();
            services.AddTransient<ITickerPairService, TickerPairService>();
            
            services.AddTransient<ICryptoDataProvider, CoinGeckoProvider>();
            services.AddSingleton<INotifier, DiscordNotifier>();

            services.AddHostedService<CoinGeckoRefreshService>();
            services.AddHttpClient();
            services.AddSingleton<DiscordSocketClient>();

            services.AddAutoMapper(Assembly.GetAssembly(typeof(BLL.AutoMapperProfiles.CoinProfile)));
            services.AddAutoMapper(Assembly.GetAssembly(typeof(WebAPI.AutoMapperProfiles.TickerProfile)));

            services.Configure<CoinGeckoOptions>(Configuration.GetSection(CoinGeckoOptions.Key));
            services.Configure<TickerFilterOptions>(Configuration.GetSection(TickerFilterOptions.Key));
            services.Configure<TickerPairQualityFilterOptions>(Configuration.GetSection(TickerPairQualityFilterOptions.Key));
            services.Configure<DiscordOptions>(Configuration.GetSection(DiscordOptions.Key));
            services.Configure<RefreshOptions>(Configuration.GetSection(RefreshOptions.Key));

            SqlConnectionStringBuilder csb = new()
            {
                DataSource = Configuration["Database:DataSource"],
                InitialCatalog = Configuration["Database:InitialCatalog"],
                IntegratedSecurity = bool.Parse(Configuration["Database:IntegratedSecurity"]),
                UserID = Configuration["Database:UserID"],
                Password = Configuration["Database:Password"]
            };
            services.AddDbContext<VoidContext>(options =>
                options.UseSqlServer(csb.ConnectionString)
            );

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Audience = Configuration["AAD:ResourceId"];
                    options.Authority = $"{Configuration["AAD:Instance"]}{Configuration["AAD:TenantId"]}";
                });

            services
                .AddControllers(options =>
                {
                    options.Filters.Add<ValidationFilter>();
                })  
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddFluentValidation(config =>
                {
                    config.LocalizationEnabled = false;
                    config.RegisterValidatorsFromAssemblyContaining<Startup>();
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Void.WebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Void.WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
