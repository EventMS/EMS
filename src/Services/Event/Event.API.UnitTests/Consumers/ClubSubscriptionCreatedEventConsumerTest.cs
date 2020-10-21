using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Event_Services.API.Context.Model;
using EMS.Event_Services.API.Events;
using EMS.Events;
using EMS.SharedTesting.Factories;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace EMS.Room_Services.API.UnitTests.Consumers
{
    [TestFixture]
    class ClubSubscriptionCreatedEventConsumerTest : EventConsumerTest<ClubSubscriptionCreatedEventConsumer>
    {
        private IPublishEndpoint _publishEndpoint;

        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext(true);
            _publishEndpoint = Substitute.For<IPublishEndpoint>();
            var eventService = EventServiceFactory.CreateEventService(content, _publishEndpoint);
            _consumer = new ClubSubscriptionCreatedEventConsumer(content);
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }

        [Test]
        public async Task Consume_CreatesANewPriceForEvent_UpdatesCorrectly()
        {
            SetupAnEntireClub();
            var e = CreateEvent();

            var @event = new ClubSubscriptionCreatedEvent()
            {
                ClubId = _clubSubscription.ClubId,
                ClubSubscriptionId = Guid.NewGuid(),
                ReferenceId = _clubSubscription.ClubSubscriptionId
            };

            await SendEvent(@event);


            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Events.Count(), Is.EqualTo(1));
                Assert.That(context.EventPrices.Count(), Is.EqualTo(2));
            }
        }

        [Test]
        public async Task Consume_CreatesANewSubscriptionWithNoReferences_UpdatesCorrectly()
        {
            SetupAnEntireClub();
            var e = CreateEvent();

            var @event = new ClubSubscriptionCreatedEvent()
            {
                ClubId = _clubSubscription.ClubId,
                ClubSubscriptionId = Guid.NewGuid(),
            };

            await SendEvent(@event);


            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Events.Count(), Is.EqualTo(1));
                Assert.That(context.EventPrices.Count(), Is.EqualTo(2));
            }
        }
        //Test event that does not exists. 
    }
}