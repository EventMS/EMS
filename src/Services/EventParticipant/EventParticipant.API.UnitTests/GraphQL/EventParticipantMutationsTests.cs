using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using EMS.SharedTesting.Helper;
using NSubstitute;
using NUnit.Framework;

using EMS.EventParticipant_Services.API.Context;
using EMS.EventParticipant_Services.API.Controllers.Request;
using EMS.EventParticipant_Services.API.GraphQlQueries;
using EMS.EventParticipant_Services.API.Mapper;

namespace EMS.EventParticipant_Services.API.UnitTests.GraphQL
{
    [TestFixture]
    class EventParticipantMutationsTests : BaseMutationsSetupTests<EventParticipantContext>
    {

        /*
        #region Setup
        private EventParticipantMutations _mutations;

        [SetUp]
        public void SetUp()
        {
            var mapper = CreateMapper();
            _mutations = new EventParticipantMutations(_context, _eventService, mapper, _authorizationService);

        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<EventParticipantProfile>();
            });
            return new AutoMapper.Mapper(config);
        }
        #endregion


        [Test]
        public async Task CreateEventParticipant_ValidRequest_AddedToDatabase()
        {
            var request = new SignUpEventRequest()
            {
                Name = "Te"
            };

            await _mutations.CreateEventParticipantAsync(request);

            using (var context = _factory.CreateContext())
            {
                var template1 = context.EventParticipants.FirstOrDefault(template1 => template1.Name == request.Name);
                Assert.That(template1, Is.Not.Null);
                Assert.That(context.EventParticipants.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<EventParticipantCreatedEvent>());
        }

        [Test]
        public async Task CreateEventParticipant_InvalidRequest_DatabaseFails()
        {
            var request = new SignUpEventRequest()
            {
                Name = "Test"
            };

            Assert.ThrowsAsync<ValidationException>(async () => await _mutations.CreateEventParticipantAsync(request));
            await _publish.Received(0).Publish(Arg.Any<EventParticipantCreatedEvent>());
        }
        */

    }
}
