
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.API.Context;
using Club.API.Events;
using EMS.Events;
using EMS.SharedTesting.Factories;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace Subscription.API.UnitTests.Consumers
{
    [TestFixture]
    class UserIsClubMemberConsumerTest : BaseConsumerTest<UserIsClubMemberConsumer, ClubContext>
    {
        private IPublishEndpoint _publishEndpoint;

        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext(true);
            _publishEndpoint = Substitute.For<IPublishEndpoint>();
            var eventService = EventServiceFactory.CreateEventService(content, _publishEndpoint);
            _consumer = new UserIsClubMemberConsumer(content, eventService);
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }

        [Test]
        public async Task Consume_UserIsAlreadyMember_NoChanges()
        {
            var @event = new UserIsClubMemberEvent()
            {
                ClubId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(new Club.API.Context.Model.Club()
                {
                    Name = "Request",
                    AccountNumber = "12345678",
                    Address = "Adresse",
                    Description = "Beskrivelse",
                    PhoneNumber = "12345678",
                    RegistrationNumber = "1234",
                    ClubId = @event.ClubId,
                    AdminId = Guid.NewGuid(),
                    InstructorIds = new HashSet<Guid>(){
                        @event.UserId
                    }
                });
                context.SaveChanges();
            }

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                var club = context.Clubs.FirstOrDefault(club => club.ClubId == @event.ClubId);
                Assert.That(club.InstructorIds.Count, Is.EqualTo(1));
            }

            await _publishEndpoint.Received(0).Publish(Arg.Any<InstructorAddedEvent>());
        }

        [Test]
        public async Task Consume_UserIsNotMember_UserIsAddedAsInstructor()
        {
            var @event = new UserIsClubMemberEvent()
            {
                ClubId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(new Club.API.Context.Model.Club()
                {
                    Name = "Request",
                    AccountNumber = "12345678",
                    Address = "Adresse",
                    Description = "Beskrivelse",
                    PhoneNumber = "12345678",
                    RegistrationNumber = "1234",
                    ClubId = @event.ClubId,
                    AdminId = Guid.NewGuid()
                });
                context.SaveChanges();
            }

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                var club = context.Clubs.FirstOrDefault(club => club.ClubId == @event.ClubId);
                Assert.That(club, Is.Not.Null);
                Assert.That(club.InstructorIds.Count, Is.EqualTo(1));
            }
            await _publishEndpoint.Received(1).Publish(Arg.Any<InstructorAddedEvent>());
        }

        [Test]
        public async Task Consume_ClubDoesNotExist_IgnoresMessage()
        {
            var @event = new UserIsClubMemberEvent()
            {
                ClubId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            await SendEvent(@event);
            await _publishEndpoint.Received(0).Publish(Arg.Any<InstructorAddedEvent>());
        }
    }
}
