using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using EMS.Room_Services.API.Context;
using EMS.Room_Services.API.Context.Model;
using EMS.TemplateWebHost.Customization.EventService;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EMS.Room_Services.API.Events
{
    public class VerifyChangedTimeslotEventConsumer :
        IConsumer<VerifyChangedTimeslotEvent>
    {

        private readonly RoomContext _roomContext;
        private readonly IEventService _eventService;

        public VerifyChangedTimeslotEventConsumer(RoomContext roomContext, IEventService eventService)
        {
            _roomContext = roomContext;
            _eventService = eventService;
        }


        private bool CollisionInBookings(List<Booking> bookings, Booking booking)
        {
            foreach (var booked in bookings)
            {
                if (booked.StartTime < booking.EndTime && booking.StartTime < booked.EndTime)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task Consume(ConsumeContext<VerifyChangedTimeslotEvent> context)
        {
            var bookingsToRemove = _roomContext.Bookings.Where(b => b.EventId == context.Message.EventId);
            _roomContext.Bookings.RemoveRange(bookingsToRemove);
            foreach (var roomId in context.Message.RoomIds)
            {
                var room = await _roomContext.Rooms
                    .Include(room => room.Bookings)
                    .FirstOrDefaultAsync(room => room.RoomId == roomId);
                if (room == null)
                {
                    var @eventFailed = new TimeslotReservationFailedEvent()
                    {
                        EventId = context.Message.EventId,
                        Reason = "Room does not exist"
                    };
                    await _eventService.SaveEventAndDbContextChangesAsync(@eventFailed);
                    await _eventService.PublishEventAsync(@eventFailed);
                    return;
                }


                var booking = new Booking()
                {
                    EndTime = context.Message.EndTime,
                    StartTime = context.Message.StartTime
                };

                if (CollisionInBookings(room.Bookings, booking))
                {
                    var @eventFailed = new TimeslotReservationFailedEvent()
                    {
                        EventId = context.Message.EventId,
                        Reason = "Timeslot already reserved"
                    };
                    await _eventService.SaveEventAndDbContextChangesAsync(@eventFailed);
                    await _eventService.PublishEventAsync(@eventFailed);
                    return;
                }
            }

            foreach (var roomId in context.Message.RoomIds)
            {
                var room = await _roomContext.Rooms
                    .Include(room => room.Bookings)
                    .FirstOrDefaultAsync(room => room.RoomId == roomId);

                var booking = new Booking()
                {
                    RoomId = room.RoomId,
                    EventId = context.Message.EventId,
                    EndTime = context.Message.EndTime,
                    StartTime = context.Message.StartTime
                };
                room.Bookings.Add(booking);
            }


            var @event = new TimeslotReservedEvent()
            {
                EventId = context.Message.EventId,
            };

            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
        }
    }
}
