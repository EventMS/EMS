using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.ClubMember_Services.API.Context;
using EMS.ClubMember_Services.API.Events;
using EMS.Events;
using EMS.Subscription_Services.API.UnitTests.Consumers;
using NUnit.Framework;

namespace EMS.ClubMember_Services_Services.API.UnitTests.Consumers
{
    [TestFixture]
    class ClubSubscriptionCreatedEventConsumerTest : BaseConsumerTest<ClubSubscriptionCreatedEventConsumer, ClubMemberContext>
    {
        [SetUp]
        public void SetUp()
        {
            _consumer = new ClubSubscriptionCreatedEventConsumer(_factory.CreateContext());
            
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }

        [Test]
        public async Task Consume_ClubSubscriptionHaventBeenCreated_ClubSubscriptionHaveBeenAdded()
        {
            var @event = new ClubSubscriptionCreatedEvent()
            {
                ClubId = Guid.NewGuid(),
                Name = "Subscription"
            };

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                var club = context.ClubSubscriptions.FirstOrDefault(club => club.ClubId == @event.ClubId && club.NameOfSubscription == @event.Name);
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
                Name = "Subscription"
            };

            await SendEvent(@event);
            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                var club = context.ClubSubscriptions.FirstOrDefault(club => club.ClubId == @event.ClubId && club.NameOfSubscription == @event.Name);
                Assert.That(club, Is.Not.Null);
                Assert.That(context.ClubSubscriptions.Count(), Is.EqualTo(1));
            }
        }
    }
}
