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

namespace EMS.EventParticipant_Services.API.Context
{
    public class EventParticipantContextSeed
    {
        public async Task SeedAsync(EventParticipantContext context, IWebHostEnvironment env, IOptions<BaseSettings> settings, ILogger<EventParticipantContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(EventParticipantContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!context.EventParticipants.Any())
                {
                    await context.EventParticipants.AddRangeAsync(GetPreconfiguredEventParticipant());
                    await context.SaveChangesAsync();
                }

            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<EventParticipantContextSeed> logger, string prefix, int retries = 3)
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

        private IEnumerable<Model.EventParticipant> GetPreconfiguredEventParticipant()
        {
            return new List<Model.EventParticipant>()
            {

            };
        }

    }
}
