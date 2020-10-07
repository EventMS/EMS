using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using EMS.Room_Services.API.Context;
using EMS.Room_Services.API.Context.Model;
using EMS.TemplateWebHost.Customization.EventService;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EMS.Room_Services.API.Events
{
    public class VerifyAvailableTimeslotEventConsumer :
        IConsumer<VerifyAvailableTimeslotEvent>
    {

        private readonly RoomContext _roomContext;
        private readonly IEventService _eventService;

        public VerifyAvailableTimeslotEventConsumer(RoomContext roomContext, IEventService eventService)
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

        public async Task Consume(ConsumeContext<VerifyAvailableTimeslotEvent> context)
        {
            var room = await _roomContext.Rooms
                .Include(room => room.Bookings)
                .FirstOrDefaultAsync(room => room.RoomId == context.Message.RoomId);
            if (room == null)
            {
                var @eventFailed = new TimeslotReservationFailed()
                {
                    EventId = context.Message.EventId,
                    RoomId = context.Message.RoomId,
                    Reason = "Room does not exist"
                };
                await _eventService.SaveEventAndDbContextChangesAsync(@eventFailed);
                await _eventService.PublishEventAsync(@eventFailed);
            }

            var booking = new Booking()
            {
                EventId = context.Message.EventId,
                EndTime = context.Message.EndTime,
                RoomId = context.Message.RoomId,
                StartTime = context.Message.StartTime
            };

            if (CollisionInBookings(room.Bookings, booking))
            {
                var @eventFailed = new TimeslotReservationFailed()
                {
                    EventId = context.Message.EventId,
                    RoomId = context.Message.RoomId,
                    Reason = "Timeslot already reserved"
                };
                await _eventService.SaveEventAndDbContextChangesAsync(@eventFailed);
                await _eventService.PublishEventAsync(@eventFailed);
            }

            room.Bookings.Add(booking);

            var @event = new TimeslotReserved()
            {
                EventId = context.Message.EventId,
                RoomId = context.Message.RoomId
            };

            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
        }
    }
}
