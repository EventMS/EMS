

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
using Stripe;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;
using EMS.Club_Service_Services.API;
using EMS.TemplateWebHost.Customization;

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
            var subscription = _mapper.Map<ClubSubscription>(request);

            _context.ClubSubscriptions.Add(subscription);
            var @event = _mapper.Map<ClubSubscriptionCreatedEvent>(subscription);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
            return subscription;
        }
        
        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<ClubSubscription> UpdateClubSubscriptionAsync(Guid id, UpdateClubSubscriptionRequest request)
        {
            var subscription = await _context.ClubSubscriptions.FindOrThrowAsync(id);

            await IsAdminIn(subscription.ClubId);
            _mapper.Map(request, subscription);
            _context.ClubSubscriptions.Update(subscription);

            var @event = _mapper.Map<ClubSubscriptionUpdatedEvent>(subscription);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
            return subscription;
        }
    }
}
