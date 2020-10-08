using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using EMS.SharedTesting.Helper;
using NSubstitute;
using NUnit.Framework;

using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Context.Model;
using EMS.Event_Services.API.Controllers.Request;
using EMS.Event_Services.API.GraphQlQueries;
using EMS.Event_Services.API.Mapper;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;

namespace EMS.Event_Services.API.UnitTests.GraphQL
{
    
    [TestFixture]
    class EventMutationsTests : BaseMutationsSetupTests<EventContext>
    {
        
        #region Setup
        private EventMutations _mutations;
        private Club _club;
        private Room _room;
        private ClubSubscription _clubSubscription;
        private Instructor _instructor;

        [SetUp]
        public void SetUp()
        {
            var mapper = CreateMapper();
            _mutations = new EventMutations(_context, _eventService, mapper);
            SetupAnEntireClub();
        }

        protected void SetupAnEntireClub()
        {
            _club = new Club()
            {
                ClubId = Guid.NewGuid()
            };
            _room = new Room()
            {
                ClubId = _club.ClubId,
                RoomId = Guid.NewGuid()
            };
            _clubSubscription = new ClubSubscription()
            {
                ClubId = _club.ClubId,
                ClubSubscriptionId = Guid.NewGuid()
            };
            _instructor = new Instructor()
            {
                ClubId = _club.ClubId,
                InstructorId = Guid.NewGuid()
            };

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(_club);
                context.Rooms.Add(_room);
                context.Instructors.Add(_instructor);
                context.Subscriptions.Add(_clubSubscription);
                context.SaveChanges();
            }
        }

        protected IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<EventProfile>();
            });
            return new AutoMapper.Mapper(config);
        }
        #endregion


        public CreateEventRequest BasicCreateRequest()
        {
            return new CreateEventRequest()
            {
                Name = "Test",
                ClubId = _club.ClubId,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                SubscriptionEventPrices = new List<SubscriptionEventPriceRequest>()
                {
                    new SubscriptionEventPriceRequest()
                    {
                        SubscriptionId = _clubSubscription.ClubSubscriptionId,
                        Price = 50
                    }
                },
                Description = "Some description",
                InstructorForEvents = new List<Guid>()
                {
                    _instructor.InstructorId
                },
                Locations = new List<Guid>()
                {
                    _room.RoomId
                }
            };
        }

        [Test]
        public async Task CreateEvent_ValidRequest_AddedToDatabase()
        {
            var request = BasicCreateRequest();

            await _mutations.CreateEventAsync(request);
            
            using (var context = _factory.CreateContext())
            {
                var e = context.Events.Include(e => e.InstructorForEvents)
                    .Include(e => e.Locations)
                    .Include(e => e.SubscriptionEventPrices)
                    .FirstOrDefault(e => e.Name == request.Name);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.Locations.Count, Is.EqualTo(1));
                Assert.That(e.InstructorForEvents.Count, Is.EqualTo(1));
                Assert.That(e.SubscriptionEventPrices.Count, Is.EqualTo(1));
                Assert.That(context.Events.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<VerifyAvailableTimeslotEvent>());
        }

        [Test]
        public async Task CreateEvent_CreateTwoEventWithSameNameInSameClub_SecondFails()
        {
            var request = BasicCreateRequest();

            await _mutations.CreateEventAsync(request);
            Assert.ThrowsAsync<DbUpdateException>(async () =>
                await _mutations.CreateEventAsync(request));
            await _publish.Received(1).Publish(Arg.Any<VerifyAvailableTimeslotEvent>());
        }

        [Test]
        public async Task CreateEvent_CreateTwoEventWithSameNameInDifferentClubs_DatabaseSavesBoth()
        {
            var request = BasicCreateRequest();
            await _mutations.CreateEventAsync(request);
            SetupAnEntireClub(); //Creates a new club in DB. 
            request = BasicCreateRequest();
            await _mutations.CreateEventAsync(request);
            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Events.Count(e => e.Name == request.Name), Is.EqualTo(2));
            }
            await _publish.Received(2).Publish(Arg.Any<VerifyAvailableTimeslotEvent>());
        }


        [Test]
        public async Task CreateEvent_ClubDoesNotExist_Fails()
        {
            var request = BasicCreateRequest();
            request.ClubId = Guid.NewGuid();

            Assert.ThrowsAsync<DbUpdateException>(async () =>
                await _mutations.CreateEventAsync(request));
            await _publish.Received(0).Publish(Arg.Any<VerifyAvailableTimeslotEvent>());
        }

        [Test]
        public async Task CreateEvent_RoomDoesNotExist_Fails()
        {
            var request = BasicCreateRequest();
            request.Locations.RemoveRange(0, 1);
            request.Locations.Add(Guid.NewGuid());

            Assert.ThrowsAsync<DbUpdateException>(async () =>
                await _mutations.CreateEventAsync(request));
            await _publish.Received(0).Publish(Arg.Any<VerifyAvailableTimeslotEvent>());
        }

        [Test]
        public async Task CreateEvent_SubscriptionDoesNotExist_Fails()
        {
            var request = BasicCreateRequest();
            request.SubscriptionEventPrices.RemoveRange(0, 1);
            request.SubscriptionEventPrices.Add(new SubscriptionEventPriceRequest()
            {
                Price = 50, 
                SubscriptionId = Guid.NewGuid()
            });

            Assert.ThrowsAsync<DbUpdateException>(async () =>
                await _mutations.CreateEventAsync(request));
            await _publish.Received(0).Publish(Arg.Any<VerifyAvailableTimeslotEvent>());
        }


        [Test]
        public async Task CreateEvent_InstructorDoesNotExist_Fails()
        {
            var request = BasicCreateRequest();
            request.InstructorForEvents.RemoveRange(0,1);
            request.InstructorForEvents.Add(Guid.NewGuid());

            Assert.ThrowsAsync<DbUpdateException>(async () =>
                await _mutations.CreateEventAsync(request));
            await _publish.Received(0).Publish(Arg.Any<VerifyAvailableTimeslotEvent>());
        }
    }
}
