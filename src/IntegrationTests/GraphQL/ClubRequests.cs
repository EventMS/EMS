using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Test;

namespace EMS.Template1_Services.API.UnitTests.GraphQL
{
    [TestFixture]
    public class ClubRequests : BaseIntegrationTest
    {

        protected async Task<Mutation> CreateSubscription()
        {
            var name = Guid.NewGuid().ToString().Substring(15);
            var query = new MutationQueryBuilder().WithCreateClubSubscription(
                    new ClubSubscriptionQueryBuilder().WithClubSubscriptionId()
                        ,
                    new CreateClubSubscriptionRequestInput()
                    {
                        Name = name,
                        Price = 100,
                        ClubId = currentClub
                    })
                .Build();
            return await Mutate(query);
        }

        protected async Task<Mutation> CreateMember(object subId, object userId)
        {
            var name = Guid.NewGuid().ToString().Substring(15);
            var query = new MutationQueryBuilder().WithCreateClubMember(
                    new ClubMemberQueryBuilder().WithClubSubscriptionId()
                    ,
                    new CreateClubMemberRequestInput()
                    {
                        UserId = userId,
                        ClubSubscriptionId = subId
                    })
                .Build();
            return await Mutate(query);
        }

        protected async Task<Mutation> CreateInstructor(object userId = null)
        {
            if (userId == null)
            {
                var resultUser = await CreateNewUser();
                userId = resultUser.CreateUser.User.Id;
            }
            
            var name = Guid.NewGuid().ToString().Substring(15);
            var query = new MutationQueryBuilder().WithAddInstructor(
                    new ClubQueryBuilder().WithAllFields().ExceptValidate()
                    , 
                    currentClub,
                    userId)
                .Build();
            return await Mutate(query);
        }

        protected async Task<Mutation> CreateEvent(object subId)
        {
            var name = Guid.NewGuid().ToString().Substring(15);
            var query = new MutationQueryBuilder().WithCreateEvent(
                    new EventQueryBuilder().WithEventId()
                    ,new CreateEventRequestInput
                    {
                        ClubId = currentClub,
                        Name = name,
                        StartTime = "2021-01-01",
                        EndTime = "2021-01-02",
                        Description = "Test klub description",
                        EventType = EventType.Public,
                        Locations = new List<object>()
                        {

                        },
                        EventPrices = new List<EventPriceRequestInput>()
                        {
                            new EventPriceRequestInput()
                            {
                                ClubSubscriptionId = subId,
                                Price = 50
                            }
                        }
                    })
                .Build();
            return await Mutate(query);
        }

        [Test]
        public async Task CreateCoupleSubscriptionsTest()
        {
            await CreateAuthorizedClient();
            var resultClub = await CreateAClub();
            currentClub = new Guid(resultClub.CreateClub.ClubId.ToString());
            await CreateSubscription();
            await CreateSubscription();
            await CreateSubscription();

            var result = await Query(new QueryQueryBuilder()
                .WithSubscriptionsForClub(new ClubSubscriptionQueryBuilder().WithClubSubscriptionId(), currentClub).Build());
            Assert.That(result.SubscriptionsForClub.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task CreateMembersTest()
        {
            await CreateAuthorizedClient();
            var resultClub = await CreateAClub();
            currentClub = new Guid(resultClub.CreateClub.ClubId.ToString());
            var resultSub = await CreateSubscription();
            var resultUser = await CreateNewUser();
            await CreateMember(resultSub.CreateClubSubscription.ClubSubscriptionId, resultUser.CreateUser.User.Id);
        }

        [Test]
        public async Task CreateInstructorTest()
        {
            await CreateAuthorizedClient();
            var resultClub = await CreateAClub();
            currentClub = new Guid(resultClub.CreateClub.ClubId.ToString());
            await CreateInstructor();
        }

        [Test]
        public async Task CreateEventTest()
        {
            await CreateAuthorizedClient();
            var resultClub = await CreateAClub();
            currentClub = new Guid(resultClub.CreateClub.ClubId.ToString());
            var resultSub = await CreateSubscription();
            await CreateEvent(resultSub.CreateClubSubscription.ClubSubscriptionId);
        }
    }
}