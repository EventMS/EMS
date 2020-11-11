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
    class SignUpEventSuccessEventConsumerTest : EventConsumerTest<SignUpEventSuccessEventConsumer>
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
        public async Task Consume_EventDoesNotExist_DoesNotCreateUserSignUp()
        {
            var @event = new SignUpEventSuccessEvent()
            {
                UserId = Guid.NewGuid(),
                EventId = Guid.NewGuid(),
            };

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.EventParticipants.Count(), Is.EqualTo(0));
            }
        }

        [Test]
        public async Task Consume_EventDoesExist_SignsUserUp()
        {
            var @event = new SignUpEventSuccessEvent()
            {
                UserId = Guid.NewGuid(),
                EventId = Guid.NewGuid(),
            };

            using (var context = _factory.CreateContext())
            {
                context.Events.Add(new Event()
                {
                    EventId = @event.EventId,
                    ClubId = Guid.NewGuid(),
                    PublicPrice = 10
                });
                context.SaveChanges();
            }

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.EventParticipants.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Consume_UserAlreadySignedUp_Ignores()
        {
            var @event = new SignUpEventSuccessEvent()
            {
                UserId = Guid.NewGuid(),
                EventId = Guid.NewGuid(),
            };

            using (var context = _factory.CreateContext())
            {
                context.Events.Add(new Event()
                {
                    EventId = @event.EventId,
                    ClubId = Guid.NewGuid(),
                    PublicPrice = 10
                });
                context.EventParticipants.Add(new EventParticipant()
                {
                    EventId = @event.EventId,
                    UserId = @event.UserId,
                });
                context.SaveChanges();
            }

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.EventParticipants.Count(), Is.EqualTo(1));
            }
        }
    }
}