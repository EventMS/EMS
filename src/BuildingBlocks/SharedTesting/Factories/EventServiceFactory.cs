using System.Data.Common;
using EMS.BuildingBlocks.EventLogEF;
using EMS.BuildingBlocks.EventLogEF.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using TemplateWebHost.Customization.EventService;
using TemplateWebHost.Customization.Settings;

namespace EMS.SharedTesting.Factories
{
    public class EventServiceFactory 
    {
        public static IEventService CreateEventService<TContext>(TContext context, IPublishEndpoint publishEndpoint) where TContext : DbContext
        {
            var logger = Substitute.For<ILogger<EventService<TContext>>>();
            var settings = new BaseSettings { SubscriptionClientName = "" };
            var wrapper = new OptionsWrapper<BaseSettings>(settings);
            return new EventService<TContext>(logger,
                context,
                (DbConnection c) => new EventLogService(new DbContextOptionsBuilder<EventLogContext>()
                    .UseSqlite(c)
                    .Options),
                wrapper,
                publishEndpoint);
        }
    }
}