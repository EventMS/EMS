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

namespace EMS.Event_Services.API.Context
{
    public class EventContextSeed
    {
        public async Task SeedAsync(EventContext context, IWebHostEnvironment env, IOptions<BaseSettings> settings, ILogger<EventContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(EventContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!context.Events.Any())
                {
                    await context.Events.AddRangeAsync(GetPreconfiguredEvent());
                    await context.SaveChangesAsync();
                }

            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<EventContextSeed> logger, string prefix, int retries = 3)
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

        private IEnumerable<Model.Event> GetPreconfiguredEvent()
        {
            return new List<Model.Event>()
            {
                new Model.Event() { Name = "Ma"},
                new Model.Event() { Name = "Je" },
                new Model.Event() { Name = "Si" },
            };
        }

    }
}
