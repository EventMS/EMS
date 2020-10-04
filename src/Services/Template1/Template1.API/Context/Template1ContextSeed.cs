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
using EMS.TemplateWebHost.Customization.Settings;

namespace EMS.Template1_Services.API.Context
{
    public class Template1ContextSeed
    {
        public async Task SeedAsync(Template1Context context, IWebHostEnvironment env, IOptions<BaseSettings> settings, ILogger<Template1ContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(Template1ContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!context.Template1s.Any())
                {
                    await context.Template1s.AddRangeAsync(GetPreconfiguredTemplate1());
                    await context.SaveChangesAsync();
                }

            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<Template1ContextSeed> logger, string prefix, int retries = 3)
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

        private IEnumerable<Model.Template1> GetPreconfiguredTemplate1()
        {
            return new List<Model.Template1>()
            {
                new Model.Template1() { Name = "Ma"},
                new Model.Template1() { Name = "Je" },
                new Model.Template1() { Name = "Si" },
            };
        }

    }
}
