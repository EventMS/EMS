using System;
using System.Collections.Generic;
using Club.API.Controllers.Request;
using EMS.Shared.Testing.Helpers;
using NUnit.Framework;


namespace Subscription.API.UnitTests.Requests
{
    /*
    [TestFixture]
    public class CreateClubRequestTest
    {
        private CreateClubRequest request;

        [SetUp]
        public void Setup()
        {
            request = new CreateClubRequest()
            {
                Name = "Request",
                AccountNumber = "12345678",
                Address = "Adresse",
                Description = "Beskrivelse",
                PhoneNumber = "12345678",
                RegistrationNumber = "1234",
                Locations = new List<string>()
                {
                    "Room 1",
                    "Room2"
                }
            };
        }


        [TestCase("Hej")]
        [TestCase("HejHej")]
        [TestCase("25CharactorLongIsOk232323")]
        public void Name_LengthIsValid_ValidationSuccess(string name)
        {
            var expectedNumberOfErrors = 0;
            request.Name = name;

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }

        [TestCase(null)] //Empty
        [TestCase("")] //Empty
        [TestCase("LongerThan25isNotOkayTryThisOutVeryLong")]
        public void Name_LengthIsValid_ValidationFails(string name)
        {
            var expectedNumberOfErrors = 1;
            request.Name = name;

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }
    */
    /*
    [TestCase(1)]
    [TestCase(100)] 
    [TestCase(1000000)]
    public void Price_IsValid_ValidationSuccess(int price)
    {
        var expectedNumberOfErrors = 0;
        var request = new CreateClubRequest()
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
        var request = new CreateClubRequest()
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
        var request = new CreateClubRequest()
        {
            Name = "Test",
            Price = 1,
            ClubId = Guid.Empty
        };

        Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
    }
    
}*/
}