using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using EMS.SharedTesting.Helper;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using EMS.Subscription_Services.API.Context;
using EMS.Subscription_Services.API.GraphQlQueries;
using EMS.Subscription_Services.API.GraphQlQueries.Request;
using EMS.Subscription_Services.API.Mapper;

namespace EMS.Subscription_Services.API.UnitTests.GraphQL
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
                context.Clubs.Add(new Club()
                {
                    ClubId = clubId
                });
                context.SaveChanges();
            }

            //Act
            var request = new CreateClubSubscriptionRequest()
            {
                Name = "Supership++",
                Price = 50,
                ClubId = clubId
            };
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
        public async Task CreateSubscription_NameIsReserved_DbFails()
        {
            //Arrange
            var clubId = Guid.NewGuid();
            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(new Club()
                {
                    ClubId = clubId
                });
                context.SaveChanges();
            }

            //Act
            var request = new CreateClubSubscriptionRequest()
            {
                Name = "Supership++",
                Price = 50,
                ClubId = clubId
            };
            await _mutations.CreateClubSubscriptionAsync(request);
            Assert.ThrowsAsync<DbUpdateException>(async () =>
                await _mutations.CreateClubSubscriptionAsync(request));
        }

        [Test]
        public async Task CreateSubscription_ClubDoesNotExist_DatabaseFails()
        {
            var request = new CreateClubSubscriptionRequest()
            {
                Name = "Supership++",
                Price = 50,
                ClubId = Guid.Empty
            };

            Assert.ThrowsAsync<DbUpdateException>(async () => 
                await _mutations.CreateClubSubscriptionAsync(request));
            await _publish.Received(0).Publish(Arg.Any<ClubSubscriptionCreatedEvent>());
        }

        [Test]
        public async Task UpdateSubscription_SubscriptionDoesNotExist_DatabaseFails()
        {
            var request = new UpdateClubSubscriptionRequest()
            {
                Name = "Supership++",
                Price = 50,
            };

            Assert.ThrowsAsync<QueryException>(async () =>
                await _mutations.UpdateClubSubscriptionAsync(Guid.Empty, request));
            await _publish.Received(0).Publish(Arg.Any<ClubSubscriptionCreatedEvent>());
        }


        [Test]
        public async Task UpdateSubscription_SubscriptionDoesExist_SubscriptionAreUpdated()
        {
            //Arrange
            var club = new Club()
            {
                ClubId = Guid.NewGuid()
            };
            var subscription = new ClubSubscription()
            {
                ClubId = club.ClubId,
                Price = 25,
                Name = "Cheapies"
            };
            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(club);
                context.ClubSubscriptions.Add(subscription);
                context.SaveChanges();
            }


            //Act
            var request = new UpdateClubSubscriptionRequest()
            {
                Name = "Supership++",
                Price = 50,
            };
            await _mutations.UpdateClubSubscriptionAsync(subscription.SubscriptionId, request);

            //Assert
            using (var context = _factory.CreateContext())
            {
                var updatedSubscription = context.ClubSubscriptions.FirstOrDefault(subscription => subscription.Name == request.Name 
                                                                                                   && subscription.Price == request.Price
                                                                                                   && subscription.ClubId == club.ClubId);
                Assert.That(updatedSubscription, Is.Not.Null);
                Assert.That(context.ClubSubscriptions.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<ClubSubscriptionUpdatedEvent>());
        }

    }
}
