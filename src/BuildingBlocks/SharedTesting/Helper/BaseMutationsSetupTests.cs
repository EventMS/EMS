using System;
using EMS.SharedTesting.Factories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using EMS.TemplateWebHost.Customization.EventService;

namespace EMS.SharedTesting.Helper
{
    [TestFixture]
    public class BaseMutationsSetupTests<TContext> where  TContext:DbContext
    {
        protected InMemorySqlLiteContextFactory<TContext> _factory;
        protected TContext _context;
        protected IEventService _eventService;
        protected IPublishEndpoint _publish;

        [SetUp]
        public void SetUp()
        {
            _factory = new InMemorySqlLiteContextFactory<TContext>(options => Activator.CreateInstance(typeof(TContext), options) as TContext);
            _context = _factory.CreateContext(true);
            _publish = Substitute.For<IPublishEndpoint>();
            _eventService = EventServiceFactory.CreateEventService(_context, _publish);
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }
    }
}