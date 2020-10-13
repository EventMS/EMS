using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace EMS.Template1_Services.API.UnitTests.GraphQL
{
    [TestFixture]
    class BasicRequests : BaseIntegrationTest
    {

        [Test]
        public async Task CreateAUser_And_LoginOnUser()
        {
            var createResult = await CreateNewUser();
            Assert.That(createResult.CreateUser.Token, Is.Not.Null);
            Assert.That(createResult.CreateUser.User.Email, Is.EqualTo(currentEmail));

            var loginResult = await LoginOnUser();
            Assert.That(loginResult.LoginUser.Token, Is.Not.Null);
            Assert.That(loginResult.LoginUser.User.Email, Is.EqualTo(currentEmail));
        }

        [Test]
        public async Task CreateAClubTest()
        {
            await CreateAuthorizedClient();
            var result = await CreateAClub();
            Assert.That(result.CreateClub.Description, Is.EqualTo("Test club"));
        }
    }
}
