﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EMS.TemplateWebHost.Customization.OutboxService
{
    /// <summary>
    /// This service implements a time that ensures that once a minute DB are checked for failed tasks. This does not do the work, just delegates.
    /// </summary>
    public class OutboxHostedService : BackgroundService
    {
        private readonly ILogger<OutboxHostedService> _logger;
        private Timer _timer;
        public OutboxHostedService(IServiceProvider services,
            ILogger<OutboxHostedService> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service running.");

            _timer = new Timer(DoWork, stoppingToken, TimeSpan.FromSeconds(60),
                TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        
        private async void DoWork(object token)
        {
            CancellationToken stoppingToken = (CancellationToken) token;

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService = scope.ServiceProvider
                    .GetRequiredService<IOutboxProcessingService>();

                await scopedProcessingService.DoWork(stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Outbox hosted service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            await base.StopAsync(stoppingToken);
        }
    }
}
