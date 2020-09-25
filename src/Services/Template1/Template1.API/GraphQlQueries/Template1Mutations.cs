using System;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Template1.API.Context;
using Template1.API.Controllers.Request;
using TemplateWebHost.Customization.EventService;

namespace Template1.API.GraphQlQueries
{
    public class Template1Mutations
    {
        private readonly Template1Context _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public Template1Mutations(Template1Context context, IEventService template1EventService, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _eventService = template1EventService ?? throw new ArgumentNullException(nameof(template1EventService));
            _mapper = mapper;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<Context.Model.Template1> UpdateTemplate1Async(Guid id, UpdateTemplate1Request request)
        {
            var item = await _context.Template1s.SingleOrDefaultAsync(ci => ci.Template1Id == id);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            item = _mapper.Map<Context.Model.Template1>(request);
            item.Template1Id = id;
            _context.Template1s.Update(item);

            var @event = _mapper.Map<Template1UpdatedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishThroughEventBusAsync(@event);

            return item;
        }


        public async Task<Context.Model.Template1> CreateTemplate1Async(CreateTemplate1Request request)
        {
            var item = _mapper.Map<Context.Model.Template1>(request);

            _context.Template1s.Add(item);

            var @event = _mapper.Map<Template1CreatedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishThroughEventBusAsync(@event);

            return item;
        }

        public async Task<Context.Model.Template1> DeleteTemplate1Async(Guid id)
        {
            var item = await _context.Template1s.SingleOrDefaultAsync(ci => ci.Template1Id == id);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            _context.Template1s.Remove(item);

            var @event = _mapper.Map<Template1DeletedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishThroughEventBusAsync(@event);
            return item;
        }
    }
}