using System;
using System.Collections.Generic;
using AutoMapper;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Context.Model;
using EMS.Event_Services.API.Controllers.Request;
using EMS.Event_Services.API.Mapper;
using EMS.Subscription_Services.API.UnitTests.Consumers;
using MassTransit;

namespace EMS.Room_Services.API.UnitTests.Consumers
{
    class EventConsumerTest<TConsumer> : BaseConsumerTest<TConsumer, EventContext> where TConsumer: class, IConsumer
    {
        protected Club _club;
        protected Room _room;
        protected ClubSubscription _clubSubscription;
        protected Instructor _instructor;

        protected IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<EventProfile>();
            });
            return new Mapper(config);
        }

        private CreateEventRequest BasicCreateRequest()
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
    }
}