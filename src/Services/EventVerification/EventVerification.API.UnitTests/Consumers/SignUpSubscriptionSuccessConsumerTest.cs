using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using EMS.EventVerification_Services.API.Context;
using EMS.EventVerification_Services.API.Events;
using EMS.SharedTesting.Factories;
using EMS.SharedTesting.Helper;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace EMS.EventVerification_Services.API.UnitTests.Consumers
{
    [TestFixture]
    class SignUpEventSuccessEventConsumerTest : BaseConsumerTest<SignUpEventSuccessEventConsumer,
        EventVerificationContext>
    {

        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext();
            _consumer = new SignUpEventSuccessEventConsumer(content);
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }

        [Test]
        public async Task Consume_SignUpEventSuccessEvent_CreatesOneCodeForUser()
        {
            var @event = new SignUpEventSuccessEvent()
            {
                EventId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.EventVerifications.Count(), Is.EqualTo(1));
                var memberDb = context.EventVerifications.Single(ev => ev.EventId == @event.EventId && ev.UserId == @event.UserId);
                Assert.That(memberDb, Is.Not.Null);
                Assert.That(memberDb.EventVerificationId, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Consume_SignUpEventSuccessEventTwice_CreatesOneCodeForUser()
        {
            var @event = new SignUpEventSuccessEvent()
            {
                EventId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };


            await SendEvent(@event);
            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.EventVerifications.Count(), Is.EqualTo(1));
                var memberDb = context.EventVerifications.Single(ev => ev.EventId == @event.EventId && ev.UserId == @event.UserId);
                Assert.That(memberDb, Is.Not.Null);
                Assert.That(memberDb.EventVerificationId, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Consume_MultipleSignUpEventSuccessEvent_CreatesOneCodeForUsers()
        {
            var @event = new SignUpEventSuccessEvent()
            {
                EventId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var @event2 = new SignUpEventSuccessEvent()
            {
                EventId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
            await SendEvent(@event);
            await SendEvent(@event2);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.EventVerifications.Count(), Is.EqualTo(2));
                var memberDb = context.EventVerifications.Single(ev => ev.EventId == @event.EventId && ev.UserId == @event.UserId);
                Assert.That(memberDb, Is.Not.Null);
                Assert.That(memberDb.EventVerificationId, Is.EqualTo(1));

                var memberDb2 = context.EventVerifications.Single(ev => ev.EventId == @event2.EventId && ev.UserId == @event2.UserId);
                Assert.That(memberDb2, Is.Not.Null);
                Assert.That(memberDb2.EventVerificationId, Is.EqualTo(2));
            }
        }

        [Test]
        public async Task Consume_MultipleSignUpSameEventSuccess_CreatesOneCodeForUsers()
        {
            var @event = new SignUpEventSuccessEvent()
            {
                EventId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var @event2 = new SignUpEventSuccessEvent()
            {
                EventId = @event.EventId,
                UserId = Guid.NewGuid()
            };
            await SendEvent(@event);
            await SendEvent(@event2);

            using (var context = _factory.CreateContext())
            {
                var memberDb = context.EventVerifications.SingleOrDefault(ev => ev.EventId == @event.EventId && ev.UserId == @event.UserId);
                Assert.That(memberDb, Is.Not.Null);
                Assert.That(memberDb.EventVerificationId, Is.EqualTo(1));

                var memberDb2 = context.EventVerifications.SingleOrDefault(ev => ev.EventId == @event2.EventId && ev.UserId == @event2.UserId);
                Assert.That(memberDb2, Is.Not.Null);
                Assert.That(memberDb2.EventVerificationId, Is.EqualTo(2));
                Assert.That(context.EventVerifications.Count(), Is.EqualTo(2));
            }
        }
    }
}