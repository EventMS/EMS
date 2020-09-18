using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using TemplateWebHost.Customization.Settings;

namespace Permission.API.Context
{
    public class PermissionContextSeed
    {
        public async Task SeedAsync(PermissionContext context, IWebHostEnvironment env, IOptions<BaseSettings> settings, ILogger<PermissionContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(PermissionContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!context.Permissions.Any())
                {
                    await context.Permissions.AddRangeAsync(GetPreconfiguredPermission());
                    await context.SaveChangesAsync();
                }

            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<PermissionContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }

        private IEnumerable<Model.Permission> GetPreconfiguredPermission()
        {
            return new List<Model.Permission>()
            {
                new Model.Permission() { Name = "Ma"},
                new Model.Permission() { Name = "Je" },
                new Model.Permission() { Name = "Si" },
            };
        }

    }
}
