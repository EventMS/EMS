

using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using EMS.Subscription_Services.API.Context;
using EMS.Subscription_Services.API.GraphQlQueries.Request;
using EMS.TemplateWebHost.Customization.EventService;
using EMS.TemplateWebHost.Customization.StartUp;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;

namespace EMS.Subscription_Services.API.GraphQlQueries
{

    public class SubscriptionMutations : BaseMutations
    {
        protected readonly SubscriptionContext _context;
        protected readonly IMapper _mapper;
        protected readonly IEventService _eventService;

        public SubscriptionMutations(SubscriptionContext context, IEventService eventService, IMapper mapper, IAuthorizationService authorizationService) : base(authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _eventService = eventService;
        }
        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<ClubSubscription> CreateClubSubscriptionAsync(CreateClubSubscriptionRequest request)
        {
            await IsAdminIn(request.ClubId);

            var item = _mapper.Map<ClubSubscription>(request);

            _context.ClubSubscriptions.Add(item);

            var @event = _mapper.Map<ClubSubscriptionCreatedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
            return item;
        }
        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<ClubSubscription> UpdateClubSubscriptionAsync(Guid id, UpdateClubSubscriptionRequest request)
        {
            var item = await _context.ClubSubscriptions.SingleOrDefaultAsync(ci => ci.SubscriptionId == id);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            await IsAdminIn(item.ClubId);


            item.Price = request.Price;
            item.Name = request.Name;
            _context.ClubSubscriptions.Update(item);

            var @event = _mapper.Map<ClubSubscriptionUpdatedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
            return item;
        }



    }
}
