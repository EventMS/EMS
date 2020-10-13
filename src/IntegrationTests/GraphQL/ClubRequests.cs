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
    }
}