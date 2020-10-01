
using System;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using AutoMapper;
using EMS.BuildingBlocks.EventLogEF;
using EMS.BuildingBlocks.EventLogEF.Services;
using EMS.BuildingBlocks.IntegrationEventLogEF.Services;
using HealthChecks.UI.Client;
using HotChocolate;
using HotChocolate.AspNetCore;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Transactions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TemplateWebHost.Customization.Filters;
using TemplateWebHost.Customization.EventService;
using TemplateWebHost.Customization.OutboxService;
using TemplateWebHost.Customization.Settings;

namespace TemplateWebHost.Customization.StartUp
{
    public abstract class BaseStartUp<T> where T:DbContext
    {
        public BaseStartUp(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected abstract string GetName();

        public IConfiguration Configuration { get; }

        public virtual IServiceCollection AddGraphQlServices(IServiceCollection services)
        {
            return services;
        }



        public virtual void ConfigureServices(IServiceCollection services)
        {
            AddAppInsight(services);
            AddCustomMVC(services);
            AddCustomDbContext(services);
            AddIdentityServer(services);
            AddCustomOptions(services);
            AddIntegrationServices(services);
            AddMassTransitServices(services);
            AddGlobalStateInterceptor(services);
            AddCustomHealthCheck(services);
            AddCustomAuthentication(services);
            AddServices(services);
            services.AddHttpContextAccessor();
            AddGraphQlServices(services);
            services.AddAutoMapper(typeof(T));
            services.AddHostedService<OutboxHostedService>();
            services.AddScoped<IOutboxProcessingService, OutboxProcessingService<T>>();
        }

        public virtual IServiceCollection AddServices(IServiceCollection service)
        {
            return service;
        }

        public virtual IServiceCollection AddGlobalStateInterceptor(IServiceCollection service)
        {
            return service;
        }

        private IServiceCollection AddMassTransitServices(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                ConfigureMassTransit(x);
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    config.ConfigureEndpoints(context);
                    config.UseInMemoryOutbox();
                });
            });
            services.AddMassTransitHostedService();
            return services;
        }

        public virtual void ConfigureMassTransit(IServiceCollectionBusConfigurator busServices)
        {

        }



        public virtual IServiceCollection AddCustomDbContext(IServiceCollection services)
        {
            AddSqlAndConnectionResilence<T>(services);
            AddSqlAndConnectionResilence<EventLogContext>(services);
            return services;
        }

        private void AddSqlAndConnectionResilence<T2>(IServiceCollection services) where T2 : DbContext
        {
            services.AddDbContext<T2>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionString"],
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        //sqlOptions.MigrationsAssembly(typeof(T2).GetTypeInfo().Assembly.GetName().Name);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            });
        }

        public virtual IServiceCollection AddCustomOptions(IServiceCollection services)
        {
            services.Configure<BaseSettings>(Configuration);
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details."
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });

            return services;
        }

        public virtual IServiceCollection AddCustomHealthCheck(IServiceCollection services)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddSqlServer(
                    Configuration["ConnectionString"],
                    name: GetName() + "DB-check",
                    tags: new string[] { "template1db" });

            hcBuilder
                .AddRabbitMQ(
                    $"amqp://{Configuration["EventBusConnection"]}",
                    name: GetName()+"-rabbitmqbus-check",
                    tags: new string[] { "rabbitmqbus" });

            return services;
        }

        public virtual IServiceCollection AddCustomMVC(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).AddNewtonsoftJson();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            return services;
        }

        public virtual IServiceCollection AddAppInsight(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddApplicationInsightsKubernetesEnricher();

            return services;
        }

        public virtual IServiceCollection AddIntegrationServices(IServiceCollection services)
        {
            services.AddTransient<Func<DbConnection, IEventLogService>>(
                sp => (DbConnection c) => new EventLogService(c));

            services.AddTransient<IEventService, EventService<T>>();
            return services;
        }

        public virtual IServiceCollection AddIdentityServer(IServiceCollection service)
        {
            return service;
        }

        public virtual IServiceCollection AddCustomAuthentication(IServiceCollection services)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            var identityUrl = Configuration.GetValue<string>("IdentityUrl");
            var securityKey = Configuration.GetValue<string>("SecurityKey");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    RequireSignedTokens = false,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey))
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
            });
            return services;
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger<BaseStartUp<T>>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }

            app.UsePlayground();
            app.UseCors("CorsPolicy");
            app.UseRouting();
            ConfigureAuth(app);
            app.UseGraphQL();

  
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });
        }

        protected virtual void ConfigureAuth(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
