using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotChocolate;
using HotChocolate.Execution;

namespace EMS.TemplateWebHost.Customization
{
    public static class MyExtensions
    {
        public static IMappingExpression<TSource, TDestination> Transform<TSource, TDestination, TSourceMember>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, object>> selector,
            Expression<Func<TSource, TSourceMember>> transform
        )
        {
            map.ForMember(selector, config =>
                config.MapFrom(transform));
            return map;
        }

        public static async Task<T> FindOrThrowAsync<T>(this DbSet<T> set, params object[] keyValues) where T : class
        {
            var result = await set.FindAsync(keyValues);
            if (result == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown for type: " + typeof(T))
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            return result;
        }

        public static async Task<TSource> SingleOrThrowAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            [NotNull] Expression<Func<TSource, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var result = await source.SingleOrDefaultAsync(predicate, cancellationToken: cancellationToken);
            if (result == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            return result;
        }
    }

    public static class HostExtensions
    {
        public static bool IsInKubernetes(this IHost host)
        {
            var cfg = host.Services.GetService<IConfiguration>();
            var orchestratorType = cfg.GetValue<string>("OrchestratorType");
            return orchestratorType?.ToUpper() == "K8S";
        }

        public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            var underK8s = host.IsInKubernetes();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<TContext>>();

                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                    if (underK8s)
                    {
                        InvokeSeeder(seeder, context, services);
                    }
                    else
                    {
                        var retries = 10;
                        var retry = Policy.Handle<SqlException>()
                            .WaitAndRetry(
                                retryCount: retries,
                                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                onRetry: (exception, timeSpan, retry, ctx) =>
                                {
                                    logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", nameof(TContext), exception.GetType().Name, exception.Message, retry, retries);
                                });

                        //if the sql server container is not created on run docker compose this
                        //migration can't fail for network related exception. The retry options for DbContext only 
                        //apply to transient exceptions
                        // Note that this is NOT applied when running some orchestrators (let the orchestrator to recreate the failing service)
                        retry.Execute(() => InvokeSeeder(seeder, context, services));
                    }

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
                    if (underK8s)
                    {
                        throw;          // Rethrow under k8s because we rely on k8s to re-run the pod
                    }
                }
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
            where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
