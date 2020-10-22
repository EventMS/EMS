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

namespace EMS.EventVerification_Services.API.Context
{
    public class EventVerificationContextSeed
    {
        public async Task SeedAsync(EventVerificationContext context, IWebHostEnvironment env, IOptions<BaseSettings> settings, ILogger<EventVerificationContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(EventVerificationContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!context.EventVerifications.Any())
                {
                    await context.EventVerifications.AddRangeAsync(GetPreconfiguredEventVerification());
                    await context.SaveChangesAsync();
                }

            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<EventVerificationContextSeed> logger, string prefix, int retries = 3)
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

        private IEnumerable<Model.EventVerification> GetPreconfiguredEventVerification()
        {
            return new List<Model.EventVerification>()
            {

            };
        }

    }
}
