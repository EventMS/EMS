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

        private async Task FailureResponse(ConsumeContext<VerifyAvailableTimeslotEvent> context, string reason)
        {
            _roomContext.ChangeTracker.Entries()
                .Where(e => e.Entity != null).ToList()
                .ForEach(e => e.State = EntityState.Detached);
            var @eventFailed = new TimeslotReservationFailedEvent()
            {
                EventId = context.Message.EventId,
                Reason = reason
            };

            await _eventService.SaveEventAndDbContextChangesAsync(@eventFailed);
            await _eventService.PublishEventAsync(@eventFailed);
        }

        public async Task Consume(ConsumeContext<VerifyAvailableTimeslotEvent> context)
        {
            if (context.Message.RoomIds != null)
            {
                foreach (var roomId in context.Message.RoomIds)
                {
                    var room = await _roomContext.Rooms
                        .Include(room => room.Bookings)
                        .FirstOrDefaultAsync(room => room.RoomId == roomId);

                    if (room == null)
                    {
                        await FailureResponse(context,"Room does not exist");
                        return;
                    }
                    var booking = new Booking()
                    {
                        EventId = context.Message.EventId,
                        EndTime = context.Message.EndTime,
                        StartTime = context.Message.StartTime
                    };
                    room.Bookings.Add(booking);
                    _roomContext.Rooms.Update(room);
                }
            }

            try
            {
                
                var @event = new TimeslotReservedEvent()
                {
                    EventId = context.Message.EventId,
                };
                await _eventService.SaveEventAndDbContextChangesAsync(@event);
                await _eventService.PublishEventAsync(@event);
            }
            catch
            {
                await FailureResponse(context, "Timeslot already reserved");
            }

        }
    }
}
