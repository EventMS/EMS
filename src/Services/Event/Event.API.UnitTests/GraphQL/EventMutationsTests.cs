using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using EMS.SharedTesting.Helper;
using NSubstitute;
using NUnit.Framework;

using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Controllers.Request;
using EMS.Event_Services.API.GraphQlQueries;
using EMS.Event_Services.API.Mapper;

namespace EMS.Event_Services.API.UnitTests.GraphQL
{
    /*
    [TestFixture]
    class EventMutationsTests : BaseMutationsSetupTests<EventContext>
    {
        
        #region Setup
        private EventMutations _mutations;

        [SetUp]
        public void SetUp()
        {
            var mapper = CreateMapper();
            _mutations = new EventMutations(_context, _eventService, mapper);

        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<EventProfile>();
            });
            return new AutoMapper.Mapper(config);
        }
        #endregion


        [Test]
        public async Task CreateEvent_ValidRequest_AddedToDatabase()
        {
            var request = new CreateEventRequest()
            {
                Name = "Te"
            };

            await _mutations.CreateEventAsync(request);

            using (var context = _factory.CreateContext())
            {
                var template1 = context.Events.FirstOrDefault(template1 => template1.Name == request.Name);
                Assert.That(template1, Is.Not.Null);
                Assert.That(context.Events.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<EventCreatedEvent>());
        }

        [Test]
        public async Task CreateEvent_InvalidRequest_DatabaseFails()
        {
            var request = new CreateEventRequest()
            {
                Name = "Test"
            };

            Assert.ThrowsAsync<ValidationException>(async () => await _mutations.CreateEventAsync(request));
            await _publish.Received(0).Publish(Arg.Any<EventCreatedEvent>());
        }


    }*/
}
