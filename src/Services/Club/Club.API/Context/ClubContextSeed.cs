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

namespace EMS.Club_Service.API.Context
{
    public class ClubContextSeed
    {
        public async Task SeedAsync(ClubContext context, IWebHostEnvironment env, IOptions<BaseSettings> settings, ILogger<ClubContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(ClubContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!context.Clubs.Any())
                {
                    await context.Clubs.AddRangeAsync(GetPreconfiguredClub());
                    await context.SaveChangesAsync();
                }

            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<ClubContextSeed> logger, string prefix, int retries = 3)
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

        private IEnumerable<Model.Club> GetPreconfiguredClub()
        {
            return new List<Model.Club>()
            {
            };
        }

    }
}
