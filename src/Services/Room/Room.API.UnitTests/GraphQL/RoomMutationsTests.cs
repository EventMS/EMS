using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using EMS.SharedTesting.Helper;
using NSubstitute;
using NUnit.Framework;

using EMS.Room_Services.API.Context;
using EMS.Room_Services.API.Context.Model;
using EMS.Room_Services.API.Controllers.Request;
using EMS.Room_Services.API.GraphQlQueries;
using EMS.Room_Services.API.Mapper;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;

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
                Name = "Te",
                ClubId = Guid.NewGuid()
            };

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(new Club()
                {
                    ClubId = request.ClubId
                });
                context.SaveChanges();
            }

            await _mutations.CreateRoomAsync(request);

            using (var context = _factory.CreateContext())
            {
                var room = context.Rooms.FirstOrDefault(room => room.Name == request.Name);
                Assert.That(room, Is.Not.Null);
                Assert.That(context.Rooms.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<RoomCreatedEvent>());
        }

        [Test]
        public async Task CreateRoom_InvalidRequest_DatabaseFails()
        {
            var request = new CreateRoomRequest()
            {
                Name = "Te",
                ClubId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<DbUpdateException>(async () => await _mutations.CreateRoomAsync(request));
            await _publish.Received(0).Publish(Arg.Any<RoomCreatedEvent>());
        }

        [Test]
        public async Task UpdateRoom_ValidRequest_AddedToDatabase()
        {
            var request = new UpdateRoomRequest()
            {
                Name = "Test updated",
            };

            var club = new Club()
            {
                ClubId = Guid.NewGuid()
            };

            var room = new Room()
            {
                ClubId = club.ClubId,
                Name = "Test"
            };

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(club);
                context.Rooms.Add(room);
                context.SaveChanges();
            }

            await _mutations.UpdateRoomAsync(room.RoomId, request);

            using (var context = _factory.CreateContext())
            {
                var roomdb = context.Rooms.FirstOrDefault(room => room.Name == request.Name);
                Assert.That(roomdb, Is.Not.Null);
                Assert.That(context.Rooms.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<RoomUpdatedEvent>());
        }

        [Test]
        public async Task UpdateRoom_RoomDoesNotExist_Fails()
        {
            var request = new UpdateRoomRequest()
            {
                Name = "Test updated",
            };

            var club = new Club()
            {
                ClubId = Guid.NewGuid()
            };

            var room = new Room()
            {
                ClubId = club.ClubId,
                Name = "Test"
            };

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(club);
                context.Rooms.Add(room);
                context.SaveChanges();
            }

            Assert.ThrowsAsync<QueryException>(async () => await _mutations.UpdateRoomAsync(Guid.NewGuid(), request));

            using (var context = _factory.CreateContext())
            {
                var roomdb = context.Rooms.FirstOrDefault(roomdb => roomdb.Name == room.Name);
                Assert.That(roomdb, Is.Not.Null);
                Assert.That(context.Rooms.Count(), Is.EqualTo(1));
            }

            await _publish.Received(0).Publish(Arg.Any<RoomUpdatedEvent>());
        }

        [Test]
        public async Task DeleteRoom_RoomDoesNotExist_Fails()
        {
            var club = new Club()
            {
                ClubId = Guid.NewGuid()
            };

            var room = new Room()
            {
                ClubId = club.ClubId,
                Name = "Test"
            };

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(club);
                context.Rooms.Add(room);
                context.SaveChanges();
            }

            Assert.ThrowsAsync<QueryException>(async () => await _mutations.DeleteRoomAsync(Guid.NewGuid()));

            using (var context = _factory.CreateContext())
            {
                var roomdb = context.Rooms.FirstOrDefault(roomdb => roomdb.Name == room.Name);
                Assert.That(roomdb, Is.Not.Null);
                Assert.That(context.Rooms.Count(), Is.EqualTo(1));
            }

            await _publish.Received(0).Publish(Arg.Any<RoomDeletedEvent>());
        }

        [Test]
        public async Task DeleteRoom_RoomDoesExist_Succeeds()
        {
            var club = new Club()
            {
                ClubId = Guid.NewGuid()
            };

            var room = new Room()
            {
                ClubId = club.ClubId,
                Name = "Test"
            };

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(club);
                context.Rooms.Add(room);
                context.SaveChanges();
            }

            await _mutations.DeleteRoomAsync(room.RoomId);

            using (var context = _factory.CreateContext())
            {
                var roomdb = context.Rooms.FirstOrDefault(roomdb => roomdb.Name == room.Name);
                Assert.That(roomdb, Is.Null);
                Assert.That(context.Rooms.Count(), Is.EqualTo(0));
            }

            await _publish.Received(1).Publish(Arg.Any<RoomDeletedEvent>());
        }
    }
}
