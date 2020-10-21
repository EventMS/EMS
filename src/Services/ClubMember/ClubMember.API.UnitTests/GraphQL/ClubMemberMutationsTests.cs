using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using EMS.SharedTesting.Helper;
using NSubstitute;
using NUnit.Framework;

using EMS.ClubMember_Services.API.Context;
using EMS.ClubMember_Services.API.Context.Model;
using EMS.ClubMember_Services.API.Controllers.Request;
using EMS.ClubMember_Services.API.GraphQlQueries;
using EMS.ClubMember_Services.API.Mapper;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;

namespace EMS.ClubMember_Services.API.UnitTests.GraphQL
{
    [TestFixture]
    class ClubMemberMutationsTests : BaseMutationsSetupTests<ClubMemberContext>
    {
        
        #region Setup
        private ClubMemberMutations _mutations;

        [SetUp]
        public void SetUp()
        {
            var mapper = CreateMapper();
            _mutations = new ClubMemberMutations(_context, _eventService, mapper, _authorizationService);

        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ClubMemberProfile>();
            });
            return new AutoMapper.Mapper(config);
        }
        #endregion

      
        [Test]
        public async Task CreateClubMember_ValidRequest_AddedToDatabase()
        {
            var clubSubscription = new ClubSubscription()
            {
                ClubId = Guid.NewGuid(),
                ClubSubscriptionId = Guid.NewGuid()
            };

            using (var context = _factory.CreateContext())
            {
                await context.ClubSubscriptions.AddAsync(clubSubscription);
                await context.SaveChangesAsync();
            }


            var request = new CreateClubMemberRequest()
            {
                ClubSubscriptionId = clubSubscription.ClubSubscriptionId,
                UserId = Guid.NewGuid()
            };

            await _mutations.CreateClubMemberAsync(request);

            using (var context = _factory.CreateContext())
            {
                var clubMember = context.ClubMembers.FirstOrDefault(clubMember => clubMember.UserId == request.UserId);
                Assert.That(clubMember, Is.Not.Null);
                Assert.That(context.ClubMembers.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<ClubMemberCreatedEvent>());
        }


        [Test]
        public async Task CreateClubMember_ToTwoMemberships_AddedToDatabase()
        {
            var clubSubscription = new ClubSubscription()
            {
                ClubId = Guid.NewGuid(),
                ClubSubscriptionId = Guid.NewGuid()
            };
            var clubSubscription2 = new ClubSubscription()
            {
                ClubId = Guid.NewGuid(),
                ClubSubscriptionId = Guid.NewGuid()
            };

            using (var context = _factory.CreateContext())
            {
                await context.ClubSubscriptions.AddRangeAsync(clubSubscription, clubSubscription2);
                await context.SaveChangesAsync();
            }


            var request = new CreateClubMemberRequest()
            {
                ClubSubscriptionId = clubSubscription.ClubSubscriptionId,
                UserId = Guid.NewGuid()
            };

            var request2 = new CreateClubMemberRequest()
            {
                ClubSubscriptionId = clubSubscription2.ClubSubscriptionId,
                UserId = Guid.NewGuid()
            };

            await _mutations.CreateClubMemberAsync(request);
            await _mutations.CreateClubMemberAsync(request2);

            using (var context = _factory.CreateContext())
            {
                var clubMember = context.ClubMembers.FirstOrDefault(clubMember => clubMember.UserId == request.UserId);
                Assert.That(clubMember, Is.Not.Null);
                var clubMember2 = context.ClubMembers.FirstOrDefault(clubMember => clubMember.UserId == request2.UserId);
                Assert.That(clubMember2, Is.Not.Null);
                Assert.That(context.ClubMembers.Count(), Is.EqualTo(2));
            }

            await _publish.Received(2).Publish(Arg.Any<ClubMemberCreatedEvent>());
        }

        [Test]
        public async Task CreateClubMember_DuplicateMemberships_Fails()
        {
            var clubSubscription = new ClubSubscription()
            {
                ClubId = Guid.NewGuid(),
                ClubSubscriptionId = Guid.NewGuid()
            };

            using (var context = _factory.CreateContext())
            {
                await context.ClubSubscriptions.AddAsync(clubSubscription);
                await context.SaveChangesAsync();
            }


            var request = new CreateClubMemberRequest()
            {
                ClubSubscriptionId = clubSubscription.ClubSubscriptionId,
                UserId = Guid.NewGuid()
            };
            await _mutations.CreateClubMemberAsync(request);
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _mutations.CreateClubMemberAsync(request));

            await _publish.Received(1).Publish(Arg.Any<ClubMemberCreatedEvent>());
        }


        [Test]
        public async Task UpdateClubMember_ValidRequest_DatabaseUpdates()
        {
            var clubSubscription = new ClubSubscription()
            {
                ClubId = Guid.NewGuid(),
                ClubSubscriptionId = Guid.NewGuid()
            };
            var clubSubscription2 = new ClubSubscription()
            {
                ClubId = clubSubscription.ClubId,
                ClubSubscriptionId = Guid.NewGuid()
            };
            var clubMember = new ClubMember()
            {
                ClubId = clubSubscription.ClubId,
                UserId = Guid.NewGuid(),
                ClubSubscriptionId = clubSubscription.ClubSubscriptionId
            };
            using (var context = _factory.CreateContext())
            {
                await context.ClubSubscriptions.AddRangeAsync(clubSubscription, clubSubscription2);
                await context.ClubMembers.AddAsync(clubMember);
                await context.SaveChangesAsync();
            }

            var request = new UpdateClubMemberRequest()
            {
                ClubId = clubSubscription2.ClubId,
                ClubSubscriptionId = clubSubscription2.ClubSubscriptionId,
                UserId = clubMember.UserId
            };

            await _mutations.UpdateClubMemberAsync(request);

            using (var context = _factory.CreateContext())
            {
                var clubMemberDb = context.ClubMembers.FirstOrDefault(clubMember => clubMember.UserId == request.UserId);
                Assert.That(clubMemberDb, Is.Not.Null);
                Assert.That(context.ClubMembers.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<ClubMemberUpdatedEvent>());
        }

        [Test]
        public async Task UpdateClubMember_ToNotExistingClubMembership_DatabaseUpdates()
        {
            var clubSubscription = new ClubSubscription()
            {
                ClubId = Guid.NewGuid(),
                ClubSubscriptionId = Guid.NewGuid()
            };
            var clubMember = new ClubMember()
            {
                ClubId = clubSubscription.ClubId,
                UserId = Guid.NewGuid(),
                ClubSubscriptionId = clubSubscription.ClubSubscriptionId
            };
            using (var context = _factory.CreateContext())
            {
                await context.ClubSubscriptions.AddRangeAsync(clubSubscription);
                await context.ClubMembers.AddAsync(clubMember);
                await context.SaveChangesAsync();
            }

            var request = new UpdateClubMemberRequest()
            {
                ClubId = clubSubscription.ClubId,
                ClubSubscriptionId = Guid.NewGuid(),
                UserId = clubMember.UserId
            };

            Assert.ThrowsAsync<DbUpdateException>(async () =>
                await _mutations.UpdateClubMemberAsync(request));

            await _publish.Received(0).Publish(Arg.Any<ClubMemberUpdatedEvent>());
        }

        /*
        [Test]
        public async Task DeleteClubMember_ClubMemberExists_DatabaseUpdates()
        {
            var clubSubscription = new ClubSubscription()
            {
                ClubId = Guid.NewGuid(),
                ClubSubscriptionId = Guid.NewGuid()
            };
            var clubMember = new ClubMember()
            {
                ClubId = clubSubscription.ClubId,
                UserId = Guid.NewGuid(),
                ClubSubscriptionId = clubSubscription.ClubSubscriptionId
            };
            using (var context = _factory.CreateContext())
            {
                await context.ClubSubscriptions.AddRangeAsync(clubSubscription);
                await context.ClubMembers.AddAsync(clubMember);
                await context.SaveChangesAsync();
            }

            await _mutations.DeleteClubMemberAsync(clubMember.UserId, clubMember.ClubId);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.ClubMembers.Count(), Is.EqualTo(0));
            }

            await _publish.Received(1).Publish(Arg.Any<ClubMemberDeletedEvent>());
        }

        [Test]
        public async Task DeleteClubMember_ClubMemberDoesNotExists_Fails()
        {

            Assert.ThrowsAsync<QueryException>(async () =>
                await _mutations.DeleteClubMemberAsync(Guid.NewGuid(), Guid.NewGuid()));

            await _publish.Received(0).Publish(Arg.Any<ClubMemberDeletedEvent>());
        }*/
    }
}
