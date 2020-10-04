

using System;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using EMS.Subscription_Services.API.Context;
using EMS.Subscription_Services.API.GraphQlQueries.Request;
using EMS.TemplateWebHost.Customization.EventService;

namespace EMS.Subscription_Services.API.GraphQlQueries
{
    public class SubscriptionMutations
    {
        private readonly SubscriptionContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public SubscriptionMutations(SubscriptionContext context, IEventService template1EventService, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _eventService = template1EventService ?? throw new ArgumentNullException(nameof(template1EventService));
            _mapper = mapper;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [Authorize(Roles = new[] { "Admin" })]
        public async Task<ClubSubscription> CreateClubSubscriptionAsync(CreateClubSubscriptionRequest request)
        {
            var item = _mapper.Map<ClubSubscription>(request);

            _context.ClubSubscriptions.Add(item);

            var @event = _mapper.Map<ClubSubscriptionCreatedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
            return item;
        }

        //This is a bug of the current way of doing it. We CANNOT decode a context that does not indikate ClubId if you have rights. But from the ID
        //The clubId is getAble... 
        [Authorize()]
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
