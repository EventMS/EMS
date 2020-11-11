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
using NUnit.Framework;

namespace EMS.EventParticipant_Services.API.UnitTests.Consumers
{
    [TestFixture]
    class ClubCreatedEventConsumerTest : EventConsumerTest<ClubCreatedEventConsumer>
    {
        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext();
            _consumer = new ClubCreatedEventConsumer(content);
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }


        [Test]
        public async Task Consume_ClubDoesNotExistUserDoesNotExist_Fails()
        {
            var @event = new ClubCreatedEvent()
            {
                ClubId = Guid.NewGuid(),
                AdminId = Guid.NewGuid()
            };

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Roles.Count(), Is.EqualTo(0));
                Assert.That(context.Clubs.Count(), Is.EqualTo(0));
            }
        }

        [Test]
        public async Task Consume_ClubDoesNotExistButUserDoes_Succeeds()
        {
            var @event = new ClubCreatedEvent()
            {
                ClubId = Guid.NewGuid(),
                AdminId = Guid.NewGuid()
            };
            using (var context = _factory.CreateContext())
            {
                context.Users.Add(new User()
                {
                    UserId = @event.AdminId
                });
                context.SaveChanges();
            }
            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Roles.Count(), Is.EqualTo(1));
                Assert.That(context.Roles.First().UserRole, Is.EqualTo("Admin"));
                Assert.That(context.Clubs.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Consume_MemberAlreadyAdminInClub_DoesNothing()
        {
            var @event = new ClubCreatedEvent()
            {
                ClubId = Guid.NewGuid(),
                AdminId = Guid.NewGuid()
            };
            using (var context = _factory.CreateContext())
            {
                context.Users.Add(new User()
                {
                    UserId = @event.AdminId
                });
                context.Clubs.Add(new Club()
                {
                    ClubId = @event.ClubId
                });
                context.Roles.Add(new Role()
                {
                    ClubId = @event.ClubId,
                    UserRole = "Admin",
                    UserId = @event.AdminId
                });
                context.SaveChanges();
            }
            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Roles.Count(), Is.EqualTo(1));
                Assert.That(context.Roles.First().UserRole, Is.EqualTo("Admin"));
                Assert.That(context.Clubs.Count(), Is.EqualTo(1));
            }
        }
    }
}