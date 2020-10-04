using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using SharedTesting.Helper;
using Subscription.API.Context;
using Subscription.API.GraphQlQueries;
using Subscription.API.GraphQlQueries.Request;
using Subscription.API.Mapper;
using Club = Subscription.API.Context.Club;

namespace Subscription.API.UnitTests.GraphQL
{
    [TestFixture]
    class SubscriptionMutationsTests : BaseMutationsSetupTests<SubscriptionContext>
    {
        
        #region Setup
        private SubscriptionMutations _mutations;

        [SetUp]
        public void SetUp()
        {
            var mapper = CreateMapper();
            _mutations = new SubscriptionMutations(_context, _eventService, mapper);

        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<SubscriptionProfile>();
            });
            return new AutoMapper.Mapper(config);
        }
        #endregion


        [Test]
        public async Task CreateSubscription_ClubIsPresentAndValidRequest_AddedToDatabase()
        {
            //Arrange
            var clubId = Guid.NewGuid();
            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(new Context.Club()
                {
                    ClubId = clubId
                });
                context.SaveChanges();
            }

            var request = new CreateClubSubscriptionRequest()
            {
                Name = "Supership++",
                Price = 50,
                ClubId = clubId
            };

            //Act
            await _mutations.CreateClubSubscriptionAsync(request);

            //Assert
            using (var context = _factory.CreateContext())
            {
                var subscription = context.ClubSubscriptions.FirstOrDefault(subscription => subscription.Name == request.Name);
                Assert.That(subscription, Is.Not.Null);
                Assert.That(context.ClubSubscriptions.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<ClubSubscriptionCreatedEvent>());
        }

        [Test]
        public async Task CreateTemplate1_ClubDoesNotExist_DatabaseFails()
        {
            var request = new CreateClubSubscriptionRequest()
            {
                Name = "Supership++",
                Price = 50,
                ClubId = Guid.Empty
            };

            Assert.ThrowsAsync<DbUpdateException>(async () => await _mutations.CreateClubSubscriptionAsync(request));
            await _publish.Received(0).Publish(Arg.Any<ClubSubscriptionCreatedEvent>());
        }


    }
}
