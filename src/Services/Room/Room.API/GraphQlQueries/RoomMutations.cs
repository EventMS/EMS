using System;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using EMS.Room_Services.API.Context;
using EMS.Room_Services.API.Context.Model;
using EMS.Room_Services.API.Controllers.Request;
using EMS.TemplateWebHost.Customization.EventService;
using EMS.TemplateWebHost.Customization.StartUp;
using Microsoft.AspNetCore.Authorization;

namespace EMS.Room_Services.API.GraphQlQueries
{
    public class RoomMutations : BaseMutations
    {
        private readonly RoomContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;


        public RoomMutations(RoomContext context, IEventService template1EventService, IMapper mapper, IAuthorizationService authorizationService) : base(authorizationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _eventService = template1EventService ?? throw new ArgumentNullException(nameof(template1EventService));
            _mapper = mapper;
        }
        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<Room> UpdateRoomAsync(Guid id, UpdateRoomRequest request)
        {
            var item = await _context.Rooms.FindAsync(id);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }
            await IsAdminIn(item.ClubId);

            _mapper.Map(request, item);
            _context.Rooms.Update(item);

            var @event = _mapper.Map<RoomUpdatedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);

            return item;
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<Room> CreateRoomAsync(CreateRoomRequest request)
        {
            await IsAdminIn(request.ClubId);
            var item = _mapper.Map<Room>(request);

            _context.Rooms.Add(item);

            var @event = _mapper.Map<RoomCreatedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);

            return item;
        }
        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<Room> DeleteRoomAsync(Guid id)
        {
            var item = await _context.Rooms.FindAsync(id);
            
            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }
            await IsAdminIn(item.ClubId);

            _context.Rooms.Remove(item);

            var @event = _mapper.Map<RoomDeletedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
            return item;
        }
    }
}
