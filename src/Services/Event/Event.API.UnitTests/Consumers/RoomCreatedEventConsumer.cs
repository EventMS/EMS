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
    class RoomCreatedEventConsumerTest : EventConsumerTest<RoomCreatedEventConsumer>
    {
        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext();
            _consumer = new RoomCreatedEventConsumer(content);
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }

        [Test]
        public async Task Consume_ClubDoesNotExist_DoesNotAddRoom()
        {
            var @event = new RoomCreatedEvent()
            {
                ClubId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
            };

            await SendEvent(@event);


            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Clubs.Count(), Is.EqualTo(0));
                Assert.That(context.Rooms.Count(), Is.EqualTo(0));
            }
        }

        [Test]
        public async Task Consume_RoomDoesNotExist_Creates()
        {
            var @event = new RoomCreatedEvent()
            {
                ClubId = Guid.NewGuid(),
                RoomId = Guid.NewGuid()
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
                Assert.That(context.Rooms.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Consume_RoomDoesExist_DoesNotCreateAnother()
        {
            var @event = new RoomCreatedEvent()
            {
                ClubId = Guid.NewGuid(),
                RoomId = Guid.NewGuid()
            };

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(new Club()
                {
                    ClubId = @event.ClubId
                });
                context.Rooms.Add(new Room()
                {
                    ClubId = @event.ClubId,
                    RoomId = @event.RoomId
                });
                context.SaveChanges();
            }

            await SendEvent(@event);


            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Rooms.Count(), Is.EqualTo(1));
            }
        }
    }
}