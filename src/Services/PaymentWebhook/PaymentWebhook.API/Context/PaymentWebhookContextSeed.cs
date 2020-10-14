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

namespace EMS.PaymentWebhook_Services.API.Context
{
    public class PaymentWebhookContextSeed
    {
        public async Task SeedAsync(PaymentWebhookContext context, IWebHostEnvironment env, IOptions<BaseSettings> settings, ILogger<PaymentWebhookContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(PaymentWebhookContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!context.PaymentWebhooks.Any())
                {
                    await context.PaymentWebhooks.AddRangeAsync(GetPreconfiguredPaymentWebhook());
                    await context.SaveChangesAsync();
                }

            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<PaymentWebhookContextSeed> logger, string prefix, int retries = 3)
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

        private IEnumerable<Model.PaymentWebhook> GetPreconfiguredPaymentWebhook()
        {
            return new List<Model.PaymentWebhook>()
            {
            };
        }

    }
}
