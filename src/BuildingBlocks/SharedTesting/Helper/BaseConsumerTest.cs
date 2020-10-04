using System;
using System.Threading.Tasks;
using EMS.BuildingBlocks.EventLogEF;
using EMS.SharedTesting.Factories;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Subscription.API.UnitTests.Consumers
{
    public class BaseConsumerTest<TConsumer, TContext> where TContext:DbContext where TConsumer: class, IConsumer
    {
        protected InMemorySqlLiteContextFactory<TContext> _factory;
        protected InMemoryTestHarness _harness;
        protected TConsumer _consumer;
        protected ConsumerTestHarness<TConsumer> _consumerHarness;


        [SetUp]
        public void SetUp()
        {
            _factory = new InMemorySqlLiteContextFactory<TContext>(options => Activator.CreateInstance(typeof(TContext), options) as TContext);
            _harness = new InMemoryTestHarness();
            _consumerHarness = _harness.Consumer(() => _consumer);
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }


        public async Task SendEvent<T>(T @event) where T : Event
        {
            await _harness.InputQueueSendEndpoint.Send(@event);
            await _harness.Consumed.Any<T>();
        }
    }
}