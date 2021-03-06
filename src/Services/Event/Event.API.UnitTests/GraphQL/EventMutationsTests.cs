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
            _mutations = new EventMutations(_context, _eventService, mapper, _authorizationService);
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

        protected Event CreateEvent(EventStatus status = EventStatus.Pending)
        {
            var @event = CreateMapper().Map<Event>(BasicCreateRequest());
            @event.Status = status;
            using (var context = _factory.CreateContext())
            {
                context.Events.Add(@event);
                context.SaveChanges();
            }

            return @event;
        }


        public CreateEventRequest BasicCreateRequest()
        {
            return new CreateEventRequest()
            {
                Name = "Test",
                ClubId = _club.ClubId,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                PublicPrice = 10,
                EventPrices = new List<EventPriceRequest>()
                {
                    new EventPriceRequest()
                    {
                        ClubSubscriptionId = _clubSubscription.ClubSubscriptionId,
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

        public UpdateEventRequest BasicUpdateRequest()
        {
            return new UpdateEventRequest()
            {
                Name = "Test Updated",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                EventPrices = new List<EventPriceRequest>()
                {
                    new EventPriceRequest()
                    {
                        ClubSubscriptionId = _clubSubscription.ClubSubscriptionId,
                        Price = 50
                    }
                },
                Description = "Some description Updated",
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
        public async Task CreateEvent_ValidRequestPublic_AddedToDatabase()
        {
            var request = BasicCreateRequest();

            var @event = await _mutations.CreateEventAsync(request);
            
            using (var context = _factory.CreateContext())
            {
                var e = context.Events.Include(e => e.InstructorForEvents)
                    .Include(e => e.Locations)
                    .Include(e => e.EventPrices)
                    .FirstOrDefault(e => e.Name == request.Name);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.Locations.Count, Is.EqualTo(1));
                Assert.That(e.InstructorForEvents.Count, Is.EqualTo(1));
                Assert.That(e.EventPrices.Count, Is.EqualTo(1));
                Assert.That(e.PublicPrice, Is.EqualTo(10));
                Assert.That(context.Events.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<VerifyAvailableTimeslotEvent>());
        }

        [Test]
        public async Task CreateEvent_ValidRequestPrivate_AddedToDatabase()
        {
            var request = BasicCreateRequest();
            request.PublicPrice = null;
            var @event = await _mutations.CreateEventAsync(request);

            using (var context = _factory.CreateContext())
            {
                var e = context.Events.Include(e => e.InstructorForEvents)
                    .Include(e => e.Locations)
                    .Include(e => e.EventPrices)
                    .FirstOrDefault(e => e.Name == request.Name);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.Locations.Count, Is.EqualTo(1));
                Assert.That(e.InstructorForEvents.Count, Is.EqualTo(1));
                Assert.That(e.EventPrices.Count, Is.EqualTo(1));
                Assert.That(e.PublicPrice, Is.Null);
                Assert.That(context.Events.Count(), Is.EqualTo(1));
            }

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
            request.EventPrices.RemoveRange(0, 1);
            request.EventPrices.Add(new EventPriceRequest()
            {
                Price = 50, 
                ClubSubscriptionId = Guid.NewGuid()
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

        [Test]
        public async Task UpdateEvent_ValidRequest_AddedToDatabase()
        {
            var request = BasicUpdateRequest();
            var @event = CreateEvent();

            await _mutations.UpdateEventAsync(@event.EventId, request);

            using (var context = _factory.CreateContext())
            {
                var e = context.Events.Include(e => e.InstructorForEvents)
                    .Include(e => e.Locations)
                    .Include(e => e.EventPrices)
                    .FirstOrDefault(e => e.Name == request.Name);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.Locations.Count, Is.EqualTo(1));
                Assert.That(e.InstructorForEvents.Count, Is.EqualTo(1));
                Assert.That(e.EventPrices.Count, Is.EqualTo(1));
                Assert.That(context.Events.Count(), Is.EqualTo(1));
                Assert.That(@event.Name, Is.Not.EqualTo(e.Name));
                Assert.That(@event.Description, Is.Not.EqualTo(e.Description));
                Assert.That(@event.InstructorForEvents.First().InstructorId, Is.EqualTo(e.InstructorForEvents.First().InstructorId));
                Assert.That(@event.Locations.First().RoomId, Is.EqualTo(e.Locations.First().RoomId));
                Assert.That(@event.EventPrices.First().ClubSubscriptionId, Is.EqualTo(e.EventPrices.First().ClubSubscriptionId));
            }

            await _publish.Received(1).Publish(Arg.Any<VerifyChangedTimeslotEvent>());
        }
    }
}
