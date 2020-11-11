using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
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
using HotChocolate.Language;
using HotChocolate.Stitching;
using HotChocolate.Stitching.Merge;
using HotChocolate.Stitching.Merge.Rewriters;
using Serilog;
using EMS.TemplateWebHost.Customization.Filters;
using EMS.TemplateWebHost.Customization.StartUp;
using HotChocolate.Stitching.Delegation;
using DirectiveLocation = HotChocolate.Types.DirectiveLocation;

namespace EMS.GraphQL.API
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

            services.AddHttpClient<PermissionService>("permission", (sp, client) =>
            {
                HttpContext context = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    client.DefaultRequestHeaders.Authorization =
                        AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"].ToString());
                }
                client.BaseAddress = new Uri(Configuration.GetValue<string>("PermissionUrl"));
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

        public void AddHttpClients(string[] https, IServiceCollection services)
        {
            foreach (var http in https)
            {
                services.AddHttpClient(http.Replace("-", ""), (sp, client) =>
                {
                    HttpContext context = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;

                    if (context != null && context.Request.Headers.ContainsKey("Authorization"))
                    {
                        client.DefaultRequestHeaders.Authorization =
                            AuthenticationHeaderValue.Parse(
                                context.Request.Headers["Authorization"]
                                    .ToString());
                    }
                    client.BaseAddress = new Uri("http://" + http);
                });
            }
        }

        public IServiceCollection AddGraphQlServices(IServiceCollection services)
        {
            services.AddDataLoaderRegistry();
            var https = Configuration.GetValue<string>("services").Split(",");
            AddHttpClients(https, services);


            services.AddStitchedSchema(builder =>
            {
                foreach (var http in https)
                {
                    builder.AddSchemaFromHttp(http.Replace("-",""));
                }
                builder.AddDocumentRewriter(RewriteDocument);
                builder.AddTypeRewriter(DelegateTypes);
                builder.AddExtensionsFromFile("./Extensions.graphql");

                builder.AddExecutionConfiguration(b =>
                {
                    b.AddErrorFilter(error => {
                        
                        if (error != null && error.Extensions != null && error.Extensions.TryGetValue("remote", out object o)
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

        private ITypeDefinitionNode DelegateTypes(ISchemaInfo s, ITypeDefinitionNode d)
        {
            return d;
        }

        private DocumentNode RewriteDocument(ISchemaInfo schema, DocumentNode definitionSchema)
        {
            var definitions = new List<IDefinitionNode>();
            var schemaName = schema.Name.Value;
            if (schemaName.Contains("api"))
            {
                schemaName = schemaName.Substring(0, schemaName.Length - 3);
            }else if(schemaName.Contains("service")){
				schemaName = schemaName.Substring(0, schemaName.Length - 7);
			}

            foreach (var definition in definitionSchema.Definitions)
            {
                if (definition is ObjectTypeDefinitionNode typeDefinition)
                {
                    if (!(typeDefinition.Name.Value.ToLower().Contains(schemaName)))
                    {
                        definitions.Add(typeDefinition.WithName(new NameNode(schemaName + "_" + typeDefinition.Name.Value)));
                        continue;
                    }
                }

                definitions.Add(definition);
            }

            return definitionSchema.WithDefinitions(definitions);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UsePlayground();
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }
            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseMiddleware<PermissionMiddleware>();
            app.UseGraphQL();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
        }

        protected string GetName()
        {
            return "GraphQL";
        }

    }
}
