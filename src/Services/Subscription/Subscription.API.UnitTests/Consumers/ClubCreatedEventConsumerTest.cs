using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using HotChocolate.Execution;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using SharedTesting.Helper;
using Subscription.API.Context;
using Subscription.API.GraphQlQueries;
using Subscription.API.GraphQlQueries.Request;
using Subscription.API.IntegrationEvents;
using Subscription.API.Mapper;
using Club = Subscription.API.Context.Club;

namespace Subscription.API.UnitTests.Consumers
{
    [TestFixture]
    class ClubCreatedEventConsumerTest : BaseConsumerTest<ClubCreatedEventConsumer, SubscriptionContext>
    {
        [SetUp]
        public void SetUp()
        {
            _consumer = new ClubCreatedEventConsumer(_factory.CreateContext());
            
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }

        [Test]
        public async Task Consume_ClubHaventBeenCreated_ClubHaveBeenAdded()
        {
            var @event = new ClubCreatedEvent()
            {
                ClubId = Guid.NewGuid() 
            };

            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                var club = context.Clubs.FirstOrDefault(club => club.ClubId == @event.ClubId);
                Assert.That(club, Is.Not.Null);
                Assert.That(context.Clubs.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Consume_ClubHaveBeenCreated_ClubAddedJustOnce()
        {
            var @event = new ClubCreatedEvent()
            {
                ClubId = Guid.NewGuid()
            };

            await SendEvent(@event);
            await SendEvent(@event);

            using (var context = _factory.CreateContext())
            {
                var club = context.Clubs.FirstOrDefault(club => club.ClubId == @event.ClubId);
                Assert.That(club, Is.Not.Null);
                Assert.That(context.Clubs.Count(), Is.EqualTo(1));
            }
        }
    }
}
