
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Event_Services.API.Context.Model;
using EMS.Event_Services.API.Events;
using EMS.Event_Services.API.Mapper;
using EMS.Events;
using EMS.SharedTesting.Factories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;

namespace EMS.Event_Services.API.UnitTests.Consumers
{
    [TestFixture]
    class TimeslotReservationFailedEventConsumerTest : EventConsumerTest<TimeslotReservationFailedEventConsumer>
    {
        private IPublishEndpoint _publishEndpoint;

        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext(true);
            _publishEndpoint = Substitute.For<IPublishEndpoint>();
            var eventService = EventServiceFactory.CreateEventService(content, _publishEndpoint);
            _consumer = new TimeslotReservationFailedEventConsumer(content, CreateMapper(), eventService);
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }

        [Test]
        public async Task Consume_EventDoesNotExistSomehow_IgnoresEvent()
        {
            SetupAnEntireClub();
            var e = CreateEvent();
            
            var @event = new TimeslotReservationFailedEvent()
            {
                EventId = Guid.NewGuid(),
                Reason = "Very bad reason"
            };

            await SendEvent(@event);

            await _publishEndpoint.Received(0).Publish(
                Arg.Any<EventCreationFailedEvent>());
        }

        [Test]
        public async Task Consume_EventDoesExist_ChangesStatus()
        {
            SetupAnEntireClub();
            var e = CreateEvent();

            var @event = new TimeslotReservationFailedEvent()
            {
                EventId = e.EventId,
                Reason = "Very bad reason"
            };

            await SendEvent(@event);

            using(var context = _factory.CreateContext())
            {
                var dbEvent = context.Events.Find(e.EventId);
                Assert.That(dbEvent.Status, Is.EqualTo(EventStatus.Failed));
            }

            await _publishEndpoint.Received(1).Publish(
                Arg.Is<EventCreationFailedEvent>(e => e.Reason=="Very bad reason"));
        }

        [Test]
        public async Task Consume_EventDoesExistInWrongStatus_IsIgnored()
        {
            SetupAnEntireClub();
            var e = CreateEvent(EventStatus.Confirmed);

            var @event = new TimeslotReservationFailedEvent()
            {
                EventId = e.EventId,
                Reason = "Very bad reason"
            };

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                var dbEvent = context.Events.Find(e.EventId);
                Assert.That(dbEvent.Status, Is.EqualTo(EventStatus.Confirmed));
            }


            await _publishEndpoint.Received(0).Publish(
                Arg.Any<EventCreationFailedEvent>());
        }
    }
}
