using System;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Club_Service_Services.API;
using EMS.Events;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using EMS.Payment_Services.API.Context;
using EMS.Payment_Services.API.Controllers.Request;
using EMS.TemplateWebHost.Customization.EventService;
using EMS.Payment_Services.API.Context.Model;
using EMS.TemplateWebHost.Customization;
using EMS.TemplateWebHost.Customization.StartUp;
using Microsoft.AspNetCore.Authorization;
using Stripe;
using Event = EMS.Payment_Services.API.Context.Model.Event;

namespace EMS.Payment_Services.API.GraphQlQueries
{
    public class PaymentMutations : BaseMutations
    {
        private readonly PaymentContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;
        private readonly StripeService _stripeService;
        private readonly IPaymentService _service;

        public PaymentMutations(PaymentContext context, IEventService template1EventService, IMapper mapper,
            IAuthorizationService authorizationService, StripeService stripeService, IPaymentService service) : base(authorizationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            ;
            _eventService = template1EventService ?? throw new ArgumentNullException(nameof(template1EventService));
            _mapper = mapper;
            _stripeService = stripeService;
            _service = service;
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<ClubSubscription> SignUpForSubscription(SignUpSubscriptionRequest request,
            [CurrentUserGlobalState] CurrentUser currentUser)
        {
            var user = await _context.Users.FindOrThrowAsync(currentUser.UserId);
            var clubSub = await _context.ClubSubscriptions.FindOrThrowAsync(request.ClubSubscriptionId);
            _stripeService.SignUserUpToSubscription(request.PaymentMethodId, user, clubSub);
            return clubSub;
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<PaymentIntentResponse> SignUpForEvent(Guid eventId,
            [CurrentUserGlobalState] CurrentUser currentUser)
        {
            var price = await _service.CalculateEventPriceForUserAsync(eventId, currentUser);
            if (price == null)
            {
                throw new QueryException(ErrorBuilder.New()
                    .SetMessage("Price is missing unexpectedly")
                    .SetCode("ID_UNKNOWN")
                    .Build());
            }

            var clientSecret = _stripeService.SignUpToEvent(price.Value, currentUser.UserId, eventId);
            return new PaymentIntentResponse()
            {
                ClientSecret = clientSecret,
                Price = price.Value
            };
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<Event> SignUpForFreeEvent(Guid eventId, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            var price = await _service.CalculateEventPriceForUserAsync(eventId, currentUser);
            if (price == null || price > 1)
            {
                throw new QueryException(ErrorBuilder.New()
                    .SetMessage("Event is not free")
                    .SetCode("ID_UNKNOWN")
                    .Build());
            }
            else
            {
                var e = new SignUpEventSuccessEvent()
                {
                    UserId = currentUser.UserId,
                    EventId = eventId
                };
                await _eventService.SaveEventAndDbContextChangesAsync(e);
                await _eventService.PublishEventAsync(e);
            }

            return await _context.Events.FindAsync(eventId);
        }
    }
}
