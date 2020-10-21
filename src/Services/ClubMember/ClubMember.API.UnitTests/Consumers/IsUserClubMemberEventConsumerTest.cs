using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.ClubMember_Services.API.Context;
using EMS.ClubMember_Services.API.Context.Model;
using EMS.ClubMember_Services.API.Events;
using EMS.ClubMember_Services.API.Mapper;
using EMS.Events;
using EMS.SharedTesting.Factories;
using EMS.SharedTesting.Helper;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace EMS.ClubMember_Services_Services.API.UnitTests.Consumers
{
    [TestFixture]
    class IsUserClubMemberEventConsumerTest : BaseConsumerTest<IsUserClubMemberEventConsumer,
        ClubMemberContext>
    {
        private IPublishEndpoint _publishEndpoint;
        
        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext(true);
            _publishEndpoint = Substitute.For<IPublishEndpoint>();
            var eventService = EventServiceFactory.CreateEventService(content, _publishEndpoint);
            _consumer = new IsUserClubMemberEventConsumer(content, eventService);
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<ClubMemberProfile>(); });
            return new AutoMapper.Mapper(config);
        }

        [Test]
        public async Task Consume_NotMember_DoesNotPublishEvent()
        {
            var @event = new IsUserClubMemberEvent()
            {
                ClubId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            await SendEvent(@event);
            await _publishEndpoint.Received(0).Publish(Arg.Any<UserIsClubMemberEvent>());
        }

        [Test]
        public async Task Consume_Member_DoesPublishEvent()
        {
            var clubmember = new ClubMember()
            {
                ClubSubscriptionId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ClubId = Guid.NewGuid()
            };
            var club = new ClubSubscription()
            {
                ClubSubscriptionId = clubmember.ClubSubscriptionId,
                ClubId = clubmember.ClubId
            };

            using (var context = _factory.CreateContext())
            {
                context.ClubMembers.Add(clubmember);
                context.ClubSubscriptions.Add(club);
                context.SaveChanges();
            }

            var @event = new IsUserClubMemberEvent()
            {
                ClubId = clubmember.ClubId,
                UserId = clubmember.UserId
            };

            await SendEvent(@event);
            await _publishEndpoint.Received(1).Publish(Arg.Any<UserIsClubMemberEvent>());
        }
    }
}