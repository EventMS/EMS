using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Controllers.Request;
using EMS.TemplateWebHost.Customization.EventService;
using EMS.Event_Services.API.Context.Model;
namespace EMS.Event_Services.API.GraphQlQueries
{
    public class EventMutations
    {
        private readonly EventContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public EventMutations(EventContext context, IEventService template1EventService, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _eventService = template1EventService ?? throw new ArgumentNullException(nameof(template1EventService));
            _mapper = mapper;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        /*
        public async Task<Event> UpdateEventAsync(Guid id, UpdateEventRequest request)
        {
            var item = await _context.Events.SingleOrDefaultAsync(ci => ci.EventId == id);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            item = _mapper.Map<Event>(request);
            item.EventId = id;
            _context.Events.Update(item);

            var @event = _mapper.Map<EventUpdatedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);

            return item;
        }*/


        public async Task<Event> CreateEventAsync(CreateEventRequest request)
        {
            var item = _mapper.Map<Event>(request);
            _context.Events.Add(item);

            var @event = _mapper.Map<VerifyAvailableTimeslotEvent>(item);
            @event.RoomIds = request.Locations;
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);

            return item;
        }
        /**/

        public async Task<Event> DeleteEventAsync(Guid eventId)
        {
            var item = await _context.Events.SingleOrDefaultAsync(ci => ci.EventId == eventId);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            _context.Events.Remove(item);

            var @event = _mapper.Map<EventDeletedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
            return item;
        }
    }
}
