using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using EMS.SharedTesting.Helper;
using NSubstitute;
using NUnit.Framework;

using EMS.Room_Services.API.Context;
using EMS.Room_Services.API.Controllers.Request;
using EMS.Room_Services.API.GraphQlQueries;
using EMS.Room_Services.API.Mapper;

namespace EMS.Room_Services.API.UnitTests.GraphQL
{
    
    [TestFixture]
    class RoomMutationsTests : BaseMutationsSetupTests<RoomContext>
    {
        
        #region Setup
        private RoomMutations _mutations;

        [SetUp]
        public void SetUp()
        {
            var mapper = CreateMapper();
            _mutations = new RoomMutations(_context, _eventService, mapper, _authorizationService);

        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<RoomProfile>();
            });
            return new AutoMapper.Mapper(config);
        }
        #endregion


        [Test]
        public async Task CreateRoom_ValidRequest_AddedToDatabase()
        {
            var request = new CreateRoomRequest()
            {
                Name = "Te"
            };

            await _mutations.CreateRoomAsync(request);

            using (var context = _factory.CreateContext())
            {
                var template1 = context.Rooms.FirstOrDefault(template1 => template1.Name == request.Name);
                Assert.That(template1, Is.Not.Null);
                Assert.That(context.Rooms.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<RoomCreatedEvent>());
        }

        [Test]
        public async Task CreateRoom_InvalidRequest_DatabaseFails()
        {
            var request = new CreateRoomRequest()
            {
                Name = "Test"
            };

            Assert.ThrowsAsync<ValidationException>(async () => await _mutations.CreateRoomAsync(request));
            await _publish.Received(0).Publish(Arg.Any<RoomCreatedEvent>());
        }


    }
}
