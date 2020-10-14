﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using EMS.Room_Services.API.Context;
using EMS.Room_Services.API.Context.Model;
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
    class VerifyAvailableTimeslotEventConsumerTest : BaseConsumerTest<VerifyAvailableTimeslotEventConsumer, RoomContext>
    {
        private IPublishEndpoint _publishEndpoint;

        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext(true);
            _publishEndpoint = Substitute.For<IPublishEndpoint>();
            var eventService = EventServiceFactory.CreateEventService(content, _publishEndpoint);
            _consumer = new VerifyAvailableTimeslotEventConsumer(content, eventService);
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }

        [Test]
        public async Task Consume_RoomDoesNotExist_PublishFailedEventWithReason()
        {
            var @event = new VerifyAvailableTimeslotEvent()
            {
                EventId = Guid.NewGuid(),
                RoomIds = new List<Guid>()
                {
                    Guid.NewGuid()
                }
            };

            await SendEvent(@event);

            await _publishEndpoint.Received(1).Publish(
                Arg.Any<TimeslotReservationFailedEvent>());
        }

        [Test]
        public async Task Consume_BookingCollision_PublishFailedEventWithReason()
        {
            var id = Guid.NewGuid();
            var @event = new VerifyAvailableTimeslotEvent()
            {
                EventId = Guid.NewGuid(),
                RoomIds = new List<Guid>()
                {
                    id
                },
                StartTime = new DateTime(2020, 1, 1, 1, 0, 0),
                EndTime = new DateTime(2020, 1,1, 2,0,0)
            };
            var club = new Club()
            {
                ClubId = Guid.NewGuid()
            };
            var room = new Room()
            {
                ClubId = club.ClubId,
                Name = "NotDefault",
                RoomId = id
            };
            var booking = new Booking()
            {
                EventId = Guid.NewGuid(),
                RoomId = id,
                StartTime = new DateTime(2020, 1, 1, 1, 30, 0),
                EndTime = new DateTime(2020, 1, 1, 2, 30, 0)
            };
            using (var context = _factory.CreateContext())
            {
                await context.Clubs.AddAsync(club);
                await context.Rooms.AddAsync(room);
                await context.Bookings.AddAsync(booking);
                await context.SaveChangesAsync();
            }

            await SendEvent(@event);

            await _publishEndpoint.Received(1).Publish(Arg.Any<TimeslotReservationFailedEvent>());
        }

        [Test]
        public async Task Consume_NoCollision_BookingAccepted()
        {
            var id = Guid.NewGuid();
            var @event = new VerifyAvailableTimeslotEvent()
            {
                EventId = Guid.NewGuid(),
                RoomIds = new List<Guid>()
                {
                    id
                },
                StartTime = new DateTime(2020, 1, 1, 1, 0, 0),
                EndTime = new DateTime(2020, 1, 1, 2, 0, 0)
            };
            var club = new Club()
            {
                ClubId = Guid.NewGuid()
            };
            var room = new Room()
            {
                ClubId = club.ClubId,
                Name = "NotDefault",
                RoomId = id
            };

            using (var context = _factory.CreateContext())
            {
                await context.Clubs.AddAsync(club);
                await context.Rooms.AddAsync(room);
                await context.SaveChangesAsync();
            }

            await SendEvent(@event);

            await _publishEndpoint.Received(1).Publish(Arg.Is<TimeslotReservedEvent>(evt =>
                 evt.EventId == @event.EventId
                ));

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Bookings.Count, Is.EqualTo(1));
            }
        }
        //Test booking of multiple rooms!

    }
}
