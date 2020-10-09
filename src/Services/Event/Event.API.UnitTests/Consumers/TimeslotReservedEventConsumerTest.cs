using System;
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
    class TimeslotReservedEventConsumerTest : EventConsumerTest<TimeslotReservedEventConsumer>
    {
        private IPublishEndpoint _publishEndpoint;

        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext(true);
            _publishEndpoint = Substitute.For<IPublishEndpoint>();
            var eventService = EventServiceFactory.CreateEventService(content, _publishEndpoint);
            _consumer = new TimeslotReservedEventConsumer(content, CreateMapper(), eventService);
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

            var @event = new TimeslotReservedEvent()
            {
                EventId = Guid.NewGuid(),
            };

            await SendEvent(@event);

            await _publishEndpoint.Received(0).Publish(
                Arg.Any<EventCreatedEvent>());
        }

        [Test]
        public async Task Consume_EventDoesExist_ChangesStatus()
        {
            SetupAnEntireClub();
            var e = CreateEvent();

            var @event = new TimeslotReservedEvent()
            {
                EventId = e.EventId,
            };

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                var dbEvent = context.Events.Find(e.EventId);
                Assert.That(dbEvent.Status, Is.EqualTo(EventStatus.CONFIRMED));
            }

            await _publishEndpoint.Received(1).Publish(
                Arg.Any<EventCreatedEvent>());
        }

        [Test]
        public async Task Consume_EventDoesExistInWrongStatus_IsIgnored()
        {
            SetupAnEntireClub();
            var e = CreateEvent(EventStatus.FAILED);

            var @event = new TimeslotReservationFailedEvent()
            {
                EventId = e.EventId,
            };

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                var dbEvent = context.Events.Find(e.EventId);
                Assert.That(dbEvent.Status, Is.EqualTo(EventStatus.FAILED));
            }


            await _publishEndpoint.Received(0).Publish(
                Arg.Any<EventCreatedEvent>());
        }
    }
}