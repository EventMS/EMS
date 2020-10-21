using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using EMS.Permission_Services.API.Context.Model;
using EMS.Permission_Services.API.Events;
using NUnit.Framework;

namespace EMS.EventParticipant_Services.API.UnitTests.Consumers
{
    [TestFixture]
    class ClubMemberUpdatedEventConsumerTest : EventConsumerTest<ClubMemberUpdatedEventConsumer>
    {
        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext();
            _consumer = new ClubMemberUpdatedEventConsumer(content);
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }


        [Test]
        public async Task Consume_ClubDoesNotExist_Fails()
        {
            var @event = new ClubMemberUpdatedEvent()
            {
                ClubId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ClubSubscriptionId = Guid.NewGuid()
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
                Assert.That(context.Roles.Count(), Is.EqualTo(0));
                Assert.That(context.Clubs.Count(), Is.EqualTo(0));
            }
        }

        [Test]
        public async Task Consume_UserDoesNotExist_Fails()
        {
            var @event = new ClubMemberUpdatedEvent()
            {
                ClubId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ClubSubscriptionId = Guid.NewGuid()
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
                Assert.That(context.Users.Count(), Is.EqualTo(0));
                Assert.That(context.Roles.Count(), Is.EqualTo(0));
                Assert.That(context.Clubs.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Consume_GetsPermission_Succeeds()
        {
            var @event = new ClubMemberUpdatedEvent()
            {
                ClubId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ClubSubscriptionId = Guid.NewGuid()
            };

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(new Club()
                {
                    ClubId = @event.ClubId
                });
                context.Roles.Add(new Role()
                {
                    UserId = @event.UserId,
                    ClubId = @event.ClubId,
                    UserRole = "Member",
                    ClubSubscriptionId = Guid.NewGuid()
                });
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
                Assert.That(context.Roles.Count(), Is.EqualTo(1));
                Assert.That(context.Roles.First().UserRole, Is.EqualTo("Member"));
                Assert.That(context.Clubs.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Consume_GetsPermissionAlreadyAdmin_Succeeds()
        {
            var @event = new ClubMemberUpdatedEvent()
            {
                ClubId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ClubSubscriptionId = Guid.NewGuid()
            };

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(new Club()
                {
                    ClubId = @event.ClubId
                });
                context.Roles.Add(new Role()
                {
                    UserId = @event.UserId,
                    ClubId = @event.ClubId,
                    UserRole = "Admin", 
                    ClubSubscriptionId = Guid.NewGuid()
                });
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
                Assert.That(context.Roles.Count(), Is.EqualTo(1));
                Assert.That(context.Roles.First().UserRole, Is.EqualTo("Admin"));
                Assert.That(context.Roles.First().ClubSubscriptionId, Is.EqualTo(@event.ClubSubscriptionId));
                Assert.That(context.Clubs.Count(), Is.EqualTo(1));
            }
        }
    }
}