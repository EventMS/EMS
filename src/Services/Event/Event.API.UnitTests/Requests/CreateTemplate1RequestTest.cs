using EMS.Shared.Testing.Helpers;
using NUnit.Framework;
using EMS.Event_Services.API.Controllers.Request;

namespace EMS.Event_Services.API.UnitTests.Requests
{
    /*
    [TestFixture]
    public class CreateEventRequestTest
    {
        [TestCase("Hej")]
        [TestCase("HejHej")]
        [TestCase("25CharactorLongIsOk232323")]
        public void Name_LengthIsValid_ValidationSuccess(string name)
        {
            var expectedNumberOfErrors = 0;
            var request = new CreateEventRequest()
            {
                Name = name,
            };
            
            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }

        [TestCase(null)] //Empty
        [TestCase("")] //Empty
        [TestCase("LongerThan25isNotOkayTryThisOutVeryLong")]
        public void Name_LengthIsValid_ValidationFails(string name)
        {
            var expectedNumberOfErrors = 1;
            var request = new CreateEventRequest()
            {
                Name = name,
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }

    }*/
}
