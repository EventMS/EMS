using System;
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
    class ClubCreatedEventConsumerTest : EventConsumerTest<ClubCreatedEventConsumer>
    {
        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext();
            _consumer = new ClubCreatedEventConsumer(content, CreateMapper());
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }

        [Test]
        public async Task Consume_ClubDoesNotExist_Creates()
        {
            var @event = new ClubCreatedEvent()
            {
                ClubId = Guid.NewGuid(),
            };

            await SendEvent(@event);


            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Clubs.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Consume_ClubDoesExist_DoesNotCreateAnother()
        {
            var @event = new ClubCreatedEvent()
            {
                ClubId = Guid.NewGuid(),
            };

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(new Club()
                {
                    ClubId = @event.ClubId
                });
                context.SaveChanges();
            }

            await SendEvent(@event);


            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Clubs.Count(), Is.EqualTo(1));
            }
        }
    }
}