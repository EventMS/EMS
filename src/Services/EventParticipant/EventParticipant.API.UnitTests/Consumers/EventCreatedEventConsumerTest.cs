using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.EventParticipant_Services.API.Context.Model;
using EMS.EventParticipant_Services.API.Events;
using EMS.Events;
using EMS.SharedTesting.Factories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;

namespace EMS.EventParticipant_Services.API.UnitTests.Consumers
{
    [TestFixture]
    class EventCreatedEventConsumerTest : EventConsumerTest<EventCreatedEventConsumer>
    {
        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext();
            _consumer = new EventCreatedEventConsumer(content, CreateMapper());
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }


        [Test]
        public async Task Consume_EventDoesNotExist_CreatesExpected()
        {
            var @event = new EventCreatedEvent()
            {
                ClubId = Guid.NewGuid(),
                EventId = Guid.NewGuid(),
                PublicPrice = 10,
                EventPrices = new List<EventPrice>()
                {
                    new EventPrice()
                    {
                        ClubSubscriptionId = Guid.NewGuid(),
                        Price = 50,
                    }
                }
            };

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Events.Count(), Is.EqualTo(1));
                var e = context.Events.Include(e => e.EventPrices)
                    .First();
                Assert.That(e.EventPrices.Count(), Is.EqualTo(1));
                Assert.That(e.PublicPrice, Is.EqualTo(10));
            }
        }

        [Test]
        public async Task Consume_EventDoesExist_DoesNotCreateAnotherd()
        {
            var @event = new EventCreatedEvent()
            {
                ClubId = Guid.NewGuid(),
                EventId = Guid.NewGuid(),
                PublicPrice = 10,
                EventPrices = new List<EventPrice>()
                {
                    new EventPrice()
                    {
                        ClubSubscriptionId = Guid.NewGuid(),
                        Price = 50,
                    }
                }
            };

            using (var context = _factory.CreateContext())
            {
                context.Events.Add(new Event()
                {
                    EventId = @event.EventId,
                    ClubId = @event.ClubId,
                    PublicPrice = 10
                });
                context.SaveChanges();
            }

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Events.Count(), Is.EqualTo(1));
                var e = context.Events.Include(e => e.EventPrices)
                    .First();
                Assert.That(e.EventPrices.Count(), Is.EqualTo(0));
                Assert.That(e.PublicPrice, Is.EqualTo(10));
            }
        }
    }
}