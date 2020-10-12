using System;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Club_Service_Services.API;
using EMS.Events;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using EMS.EventParticipant_Services.API.Context;
using EMS.EventParticipant_Services.API.Controllers.Request;
using EMS.TemplateWebHost.Customization.EventService;
using EMS.EventParticipant_Services.API.Context.Model;
using EMS.TemplateWebHost.Customization.StartUp;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using EventType = EMS.EventParticipant_Services.API.Context.Model.EventType;

namespace EMS.EventParticipant_Services.API.GraphQlQueries
{
    public class EventParticipantMutations : BaseMutations
    {
        private readonly EventParticipantContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public EventParticipantMutations(EventParticipantContext context, IEventService template1EventService, IMapper mapper, IAuthorizationService authorizationService) : base(authorizationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _eventService = template1EventService ?? throw new ArgumentNullException(nameof(template1EventService));
            _mapper = mapper;
        }

        //Do we need this type of mutation?
        //Or do all come through stripe service?
        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<EventParticipant> SignUpFreeEventAsync(Guid eventId, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            var item = _context.Events.Find(eventId);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            //Consider making event flow to membership service to get what membership the current user haves?
            //Currently this only works if it's free for ALL memberships, instead of the actual one that the user have. 
            if (item.IsFree)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The event is not free.")
                        .SetCode("REQUEST_INVALID")
                        .Build());
            }


            if (item.EventType == EventType.Private)
            {
                await IsMemberIn(item.ClubId);
            }

            var eventParticipant = new EventParticipant()
            {
                EventId = eventId,
                UserId = currentUser.UserId
            };

            _context.EventParticipants.Add(eventParticipant);

            var @event = _mapper.Map<SignUpFreeEventSuccess>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);

            return eventParticipant;
        }
    }
}
