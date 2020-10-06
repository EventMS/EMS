using EMS.Shared.Testing.Helpers;
using NUnit.Framework;
using EMS.ClubMember_Services.API.Controllers.Request;

namespace EMS.ClubMember_Services.API.UnitTests.Requests
{
    [TestFixture]
    public class UpdateClubMemberRequestTest
    {
        [TestCase("Hej")]
        [TestCase("HejHej")]
        [TestCase("25CharactorLongIsOk232323")]
        public void Name_LengthIsValid_ValidationSuccess(string name)
        {
            var expectedNumberOfErrors = 0;
            var request = new UpdateClubMemberRequest()
            {
                //Name = name,
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }

        [TestCase(null)] //Empty
        [TestCase("")] //Empty
        [TestCase("LongerThan25isNotOkayTryThisOutVeryLong")]
        public void Name_LengthIsValid_ValidationFails(string name)
        {
            var expectedNumberOfErrors = 1;
            var request = new UpdateClubMemberRequest()
            {
                //Name = name,
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }

    }
}
