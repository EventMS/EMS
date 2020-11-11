using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Test;

namespace EMS.Template1_Services.API.UnitTests.GraphQL
{
    [TestFixture]
    class CreatingResourceTesting : BaseIntegrationTest
    {

        [Test]
        public async Task CreateAUser_And_LoginOnUser()
        {
            await CreateNewUser();
            await LoginOnUser();
        }

        [Test]
        public async Task CreateAClubTest()
        {
            await CreateAuthorizedClient();
            await CreateAClub();
        }

        [Test]
        public async Task CreateCoupleSubscriptionsTest()
        {
            await CreateAuthorizedClient();
            await CreateAClub();
            await CreateSubscription();
            await CreateSubscription();
        }

        [Test]
        public async Task CreateMembersTest()
        {
            await CreateAuthorizedClient();
            await CreateAClub();
            await CreateSubscription();
            await CreateNewUser();
            await CreateMember();
        }

        [Test]
        public async Task CreateInstructorTest()
        {
            await CreateAuthorizedClient();
            await CreateAClub();
            await CreateSubscription();
            await CreateNewUser();
            await CreateMember();
            //Give event some time to reach, as it's a two step event process to hit permission service,
            //Which is where create instructor hits. 
            Thread.Sleep(1000);
            await CreateInstructor();
        }

        [Test]
        public async Task CreateEventTest()
        {
            await CreateAuthorizedClient();
            await CreateAClub();
            await CreateSubscription();
            await CreateEvent();
        }

        [Test]
        public async Task CreateManyMembersTest()
        {
            await CreateAuthorizedClient();
            await CreateAClub();
            await CreateSubscription();
            for (int i = 0; i < 10; i++)
            {
                await CreateNewUser();
                await CreateMember();
            }
        }

        [Test]

        public async Task CreateManyEventsTest()
        {
            await CreateAuthorizedClient();
            await CreateAClub();
            await CreateSubscription();
            for (int i = 0; i < 10; i++)
            {
                await CreateEvent();
            }
        }
    }
}
