using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Club_Service_Services.API;
using EMS.Events;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using EMS.EventParticipant_Services.API.Context;
using EMS.TemplateWebHost.Customization.EventService;
using EMS.EventParticipant_Services.API.Context.Model;
using EMS.TemplateWebHost.Customization;
using EMS.TemplateWebHost.Customization.StartUp;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Internal;
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

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<EventParticipant> SignUpFreeEventAsync(Guid eventId, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            var e = await _context.Events
                .Include(e => e.EventPrices)
                .SingleOrThrowAsync(e => e.EventId == eventId);

            var userSub = currentUser.GetSubscriptionIn(e.ClubId);
            if (!userSub.HasValue && e.EventType == EventType.Private)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The event is private")
                        .SetCode("USER_NOT_MEMBER")
                        .Build());
            }

            if (userSub == null)
            {
                userSub = Guid.Empty;
            }
            var userPrice = e.EventPrices.Find(ep => ep.ClubSubscriptionId == userSub.Value);
            if (userPrice == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("Price is not configured so you cannot join")
                        .SetCode("USER_CANNOT_JOIN")
                        .Build());
            }

            if (Math.Abs(userPrice.Price) < 1)
            {
                var ep = new EventParticipant()
                {
                    EventId = eventId,
                    UserId = currentUser.UserId
                };
                _context.EventParticipants.Add(ep);
                var @event = new SignUpEventSuccess()
                {
                    UserId = currentUser.UserId,
                    EventId = eventId
                };
                await _eventService.SaveEventAndDbContextChangesAsync(@event);
                await _eventService.PublishEventAsync(@event);
                return ep;
            }
            else
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("Event is not free for your membership type.")
                        .SetCode("EVENT_IS_NOT_FREE")
                        .Build());
            }
        }
    }
}
