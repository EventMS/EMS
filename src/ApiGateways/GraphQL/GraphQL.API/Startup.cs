using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.Execution;
using HotChocolate.Stitching;
using Newtonsoft.Json;
using Serilog;
using TemplateWebHost.Customization;
using TemplateWebHost.Customization.Filters;
using TemplateWebHost.Customization.StartUp;
using Path = System.IO.Path;

namespace GraphQL.API
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            AddCustomMVC(services);
            services.AddHttpContextAccessor();
            AddGraphQlServices(services);
            AddServices(services);
        }

        public virtual IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddHttpClient<PermissionService>("permission",(sp, client) =>
            {
                HttpContext context = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    client.DefaultRequestHeaders.Authorization =
                        AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"].ToString());
                }
                client.BaseAddress = new Uri("http://permission-api");
            });
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

        public IServiceCollection AddGraphQlServices(IServiceCollection services)
        {
            
            var https = Configuration.GetValue<string>("services").Split(",");
            services.AddDataLoaderRegistry();

            foreach (var http in https)
            {
                services.AddHttpClient(http.Replace("-",""), (sp, client) =>
                {
                    HttpContext context = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;

                    if (context.Request.Headers.ContainsKey("Authorization"))
                    {
                        client.DefaultRequestHeaders.Authorization =
                            AuthenticationHeaderValue.Parse(
                                context.Request.Headers["Authorization"]
                                    .ToString());
                    }
                    client.BaseAddress = new Uri("http://"+ http);
                });
            }


            services.AddStitchedSchema(builder =>
            {
                foreach (var http in https)
                {
                    builder.AddSchemaFromHttp(http.Replace("-",""));
                }

                builder.AddExecutionConfiguration(b =>
                {
                    b.AddErrorFilter(error => {
                        if (error.Extensions.TryGetValue("remote", out object o)
                            && o is IError originalError)
                        {
                            return error.AddExtension(
                                "remote_code",
                                originalError.Code);
                        }


                        return error;
                    });
                });
            });

            services.AddGraphQLSubscriptions();
            return services;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            app.UsePlayground();
            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseMiddleware<PermissionMiddleware>();
            app.UseGraphQL();
        }

        protected string GetName()
        {
            return "GraphQL";
        }

    }
}
