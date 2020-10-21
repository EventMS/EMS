using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using EMS.Permission_Services.API.Context.Model;
using EMS.Permission_Services.API.Events;
using EMS.SharedTesting.Factories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;

namespace EMS.EventParticipant_Services.API.UnitTests.Consumers
{
    [TestFixture]
    class UserCreatedEventConsumerTest : EventConsumerTest<UserCreatedEventConsumer>
    {
        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext();
            _consumer = new UserCreatedEventConsumer(content);
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }


        [Test]
        public async Task Consume_UserDoesNotExist_IsCreated()
        {
            var @event = new UserCreatedEvent()
            {
                UserId = Guid.NewGuid()
            };

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Users.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Consume_ClubDoesNotExistButUserDoes_Succeeds()
        {
            var @event = new UserCreatedEvent()
            {
                UserId = Guid.NewGuid()
            };
            using (var context = _factory.CreateContext())
            {
                context.Users.Add(new User()
                {
                    UserId = @event.UserId
                });
                context.SaveChanges();
            }

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Users.Count(), Is.EqualTo(1));
            }
        }
    }
}