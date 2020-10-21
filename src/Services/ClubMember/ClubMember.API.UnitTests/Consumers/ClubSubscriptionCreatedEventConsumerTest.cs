using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.ClubMember_Services.API.Context;
using EMS.ClubMember_Services.API.Events;
using EMS.ClubMember_Services.API.Mapper;
using EMS.Events;
using EMS.SharedTesting.Helper;
using NUnit.Framework;

namespace EMS.ClubMember_Services_Services.API.UnitTests.Consumers
{
    [TestFixture]
    class ClubSubscriptionCreatedEventConsumerTest : BaseConsumerTest<ClubSubscriptionCreatedEventConsumer, ClubMemberContext>
    {
        [SetUp]
        public void SetUp()
        {
            _consumer = new ClubSubscriptionCreatedEventConsumer(_factory.CreateContext(),CreateMapper());
            
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ClubMemberProfile>();
            });
            return new AutoMapper.Mapper(config);
        }

        [Test]
        public async Task Consume_ClubSubscriptionHaventBeenCreated_ClubSubscriptionHaveBeenAdded()
        {
            var @event = new ClubSubscriptionCreatedEvent()
            {
                ClubId = Guid.NewGuid(),
                ClubSubscriptionId = Guid.NewGuid()
            };

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                var club = context.ClubSubscriptions.FirstOrDefault(club => club.ClubId == @event.ClubId && club.ClubSubscriptionId == @event.ClubSubscriptionId);
                Assert.That(club, Is.Not.Null);
                Assert.That(context.ClubSubscriptions.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Consume_ClubSubscriptionGetsAddedOnce_ClubSubscriptionHaveBeenAdded()
        {
            var @event = new ClubSubscriptionCreatedEvent()
            {
                ClubId = Guid.NewGuid(),
                ClubSubscriptionId = Guid.NewGuid()
            };

            await SendEvent(@event);
            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                var club = context.ClubSubscriptions.FirstOrDefault(club => club.ClubId == @event.ClubId && club.ClubSubscriptionId == @event.ClubSubscriptionId);
                Assert.That(club, Is.Not.Null);
                Assert.That(context.ClubSubscriptions.Count(), Is.EqualTo(1));
            }
        }
    }
}
