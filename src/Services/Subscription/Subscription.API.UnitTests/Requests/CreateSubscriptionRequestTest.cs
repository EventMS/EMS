using System;
using EMS.SharedTesting.Helpers;
using NUnit.Framework;
using EMS.Subscription_Services.API.GraphQlQueries.Request;

namespace EMS.Subscription_Services.API.UnitTests.Requests
{
    [TestFixture]
    public class CreateSubscriptionRequestTest
    {
        [TestCase("Hej")]
        [TestCase("HejHej")]
        [TestCase("25CharactorLongIsOk232323")]
        public void Name_LengthIsValid_ValidationSuccess(string name)
        {
            var expectedNumberOfErrors = 0;
            var request = new CreateClubSubscriptionRequest()
            {
                Name = name,
                Price = 1,
                ClubId = Guid.NewGuid()
            };
            
            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }

        [TestCase(null)] //Empty
        [TestCase("")] //Empty
        [TestCase("LongerThan25isNotOkayTryThisOutVeryLong")]
        public void Name_LengthIsValid_ValidationFails(string name)
        {
            var expectedNumberOfErrors = 1;
            var request = new CreateClubSubscriptionRequest()
            {
                Name = name,
                Price = 1,
                ClubId = Guid.NewGuid()
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }

        [TestCase(1)]
        [TestCase(100)] 
        [TestCase(1000000)]
        public void Price_IsValid_ValidationSuccess(int price)
        {
            var expectedNumberOfErrors = 0;
            var request = new CreateClubSubscriptionRequest()
            {
                Name = "Test",
                Price = price,
                ClubId = Guid.NewGuid()
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }

        [TestCase(-1)]
        [TestCase(1000001)]
        public void Price_IsInvalid_ValidationFails(int price)
        {
            var expectedNumberOfErrors = 1;
            var request = new CreateClubSubscriptionRequest()
            {
                Name = "Test",
                Price = price,
                ClubId = Guid.NewGuid()
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }

        [Test]
        public void ClubId_ISEmpty_ValidationFails()
        {
            var expectedNumberOfErrors = 1;
            var request = new CreateClubSubscriptionRequest()
            {
                Name = "Test",
                Price = 1,
                ClubId = Guid.Empty
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }
    }
}