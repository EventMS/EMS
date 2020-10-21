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
    class SignUpSubscriptionSuccessConsumerTest : BaseConsumerTest<SignUpSubscriptionSuccessConsumer,
        ClubMemberContext>
    {
        private IPublishEndpoint _publishEndpoint;

        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext(true);
            _publishEndpoint = Substitute.For<IPublishEndpoint>();
            var eventService = EventServiceFactory.CreateEventService(content, _publishEndpoint);
            _consumer = new SignUpSubscriptionSuccessConsumer(content, CreateMapper(), eventService);
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
        public async Task Consume_SubscriptionMustExist_DoesNothing()
        {
            var @event = new SignUpSubscriptionSuccess()
            {
                ClubSubscriptionId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            await SendEvent(@event);
            await _publishEndpoint.Received(0).Publish(Arg.Any<ClubMemberCreatedEvent>());
            await _publishEndpoint.Received(0).Publish(Arg.Any<ClubMemberUpdatedEvent>());
        }

        [Test]
        public async Task Consume_SubscriptionExistAndNoMember_Created()
        {
            var sub = new ClubSubscription()
            {
                ClubId = Guid.NewGuid(),
                ClubSubscriptionId = Guid.NewGuid()
            };
            using (var context = _factory.CreateContext())
            {
                context.ClubSubscriptions.Add(sub);
                context.SaveChanges();
            }

            var @event = new SignUpSubscriptionSuccess()
            {
                ClubSubscriptionId = sub.ClubSubscriptionId,
                UserId = Guid.NewGuid()
            };

            await SendEvent(@event);
            using (var context = _factory.CreateContext())
            {
                Assert.That(context.ClubMembers.Count(), Is.EqualTo(1));
                var memberDb = context.ClubMembers.Find(@event.UserId, sub.ClubId);
                Assert.That(memberDb, Is.Not.Null);
                Assert.That(memberDb.ClubSubscriptionId, Is.EqualTo(sub.ClubSubscriptionId));
            }
            await _publishEndpoint.Received(1).Publish(Arg.Any<ClubMemberCreatedEvent>());
            await _publishEndpoint.Received(0).Publish(Arg.Any<ClubMemberUpdatedEvent>());
        }

        [Test]
        public async Task Consume_SubscriptionExistAndNoMember_Updated()
        {
            var sub = new ClubSubscription()
            {
                ClubId = Guid.NewGuid(),
                ClubSubscriptionId = Guid.NewGuid()
            };
            var member = new ClubMember()
            {
                ClubId = sub.ClubId,
                ClubSubscriptionId = sub.ClubSubscriptionId,
                UserId = Guid.NewGuid()

            };
            using (var context = _factory.CreateContext())
            {
                context.ClubSubscriptions.Add(sub);
                context.ClubMembers.Add(member);
                context.SaveChanges();
            }

            var @event = new SignUpSubscriptionSuccess()
            {
                ClubSubscriptionId = sub.ClubSubscriptionId,
                UserId = member.UserId
            };

            await SendEvent(@event);
            using (var context = _factory.CreateContext())
            {
                Assert.That(context.ClubMembers.Count(), Is.EqualTo(1));
                var memberDb = context.ClubMembers.Find(@event.UserId, sub.ClubId);
                Assert.That(memberDb, Is.Not.Null);
                Assert.That(memberDb.ClubSubscriptionId, Is.EqualTo(sub.ClubSubscriptionId));
            }
            await _publishEndpoint.Received(0).Publish(Arg.Any<ClubMemberCreatedEvent>());
            await _publishEndpoint.Received(1).Publish(Arg.Any<ClubMemberUpdatedEvent>());
        }
    }
}