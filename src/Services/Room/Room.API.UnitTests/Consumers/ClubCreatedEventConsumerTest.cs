
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using EMS.Room_Services.API.Context;
using EMS.Room_Services.API.Events;
using EMS.SharedTesting.Factories;
using EMS.SharedTesting.Helper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;

namespace EMS.Room_Services.API.UnitTests.Consumers
{
    [TestFixture]
    class ClubCreatedEventConsumerTest : BaseConsumerTest<ClubCreatedEventConsumer, RoomContext>
    {
        private IPublishEndpoint _publishEndpoint;

        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext(true);
            _publishEndpoint = Substitute.For<IPublishEndpoint>();
            var eventService = EventServiceFactory.CreateEventService(content, _publishEndpoint);
            _consumer = new ClubCreatedEventConsumer(content, eventService);
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }

        [Test]
        public async Task Consume_NoLocations_DefaultRoomCreated()
        {
            var @event = new ClubCreatedEvent()
            {
                ClubId = Guid.NewGuid()
            };

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                var club = context.Clubs
                    .Include(club => club.Rooms)
                    .FirstOrDefault(club => club.ClubId == @event.ClubId);
                Assert.That(club.Rooms.Count, Is.EqualTo(1));
                Assert.That(club.Rooms.First().Name, Is.EqualTo("Default"));
            }

            await _publishEndpoint.Received(1).Publish(Arg.Any<RoomCreatedEvent>());
        }

        [Test]
        public async Task Consume_1Location_ExpectedRoomCreated()
        {
            var @event = new ClubCreatedEvent()
            {
                ClubId = Guid.NewGuid(),
                Locations = new List<string>()
                {
                    "Test"
                }
            };

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                var club = context.Clubs
                    .Include(club => club.Rooms)
                    .FirstOrDefault(club => club.ClubId == @event.ClubId);
                Assert.That(club.Rooms.Count, Is.EqualTo(1));
                Assert.That(club.Rooms.First().Name, Is.EqualTo("Test"));
            }

            await _publishEndpoint.Received(1).Publish(Arg.Any<RoomCreatedEvent>());
        }


        [Test]
        public async Task Consume_2Location_ExpectedRoomsCreated()
        {
            var @event = new ClubCreatedEvent()
            {
                ClubId = Guid.NewGuid(),
                Locations = new List<string>()
                {
                    "Test",
                    "Test2"
                }
            };

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                var club = context.Clubs
                    .Include(club => club.Rooms)
                    .FirstOrDefault(club => club.ClubId == @event.ClubId);
                Assert.That(club.Rooms.Count, Is.EqualTo(2));
                Assert.That(club.Rooms.Find(room=> room.Name == "Test"), Is.Not.Null);
                Assert.That(club.Rooms.Find(room => room.Name == "Test2"), Is.Not.Null);
            }

            await _publishEndpoint.Received(2).Publish(Arg.Any<RoomCreatedEvent>());
        }
    }
}
