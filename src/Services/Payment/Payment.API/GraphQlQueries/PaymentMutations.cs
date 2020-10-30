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

namespace EMS.Payment_Services.API.GraphQlQueries
{
    public class PaymentMutations : BaseMutations
    {
        private readonly PaymentContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;
        private readonly StripeService _stripeService;

        public PaymentMutations(PaymentContext context, IEventService template1EventService, IMapper mapper, IAuthorizationService authorizationService, StripeService stripeService) : base(authorizationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _eventService = template1EventService ?? throw new ArgumentNullException(nameof(template1EventService));
            _mapper = mapper;
            _stripeService = stripeService;
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<ClubSubscription> SignUpForSubscription(SignUpSubscriptionRequest request, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            var user = await _context.Users.FindOrThrowAsync(currentUser.UserId);
            var clubSub = await _context.ClubSubscriptions.FindOrThrowAsync(request.ClubSubscriptionId);
            _stripeService.SignUserUpToSubscription(request.PaymentMethodId, user, clubSub);
            return clubSub;
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<PaymentIntentResponse> SignUpForEvent(Guid eventId, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            var price = await CalculateEventPriceForUserAsync(eventId, currentUser);
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
        public async Task<PaymentIntentResponse> SignUpForFreeEvent(Guid eventId, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            var price = await CalculateEventPriceForUserAsync(eventId, currentUser);
            if (price == null)
            {
                throw new QueryException(ErrorBuilder.New()
                    .SetMessage("Price is missing unexpectedly")
                    .SetCode("ID_UNKNOWN")
                    .Build());
            }else if (price != 0)
            {

            }
            var clientSecret = _stripeService.SignUpToEvent(price.Value, currentUser.UserId, eventId);
            return new PaymentIntentResponse()
            {
                ClientSecret = clientSecret,
                Price = price.Value
            };
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<float?> CalculateEventPriceForUserAsync(Guid eventId, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            var e = await _context.Events.FindOrThrowAsync(eventId);
            var clubPermission = currentUser.ClubPermissions?.Find(club => club.ClubId == e.ClubId);
            if (clubPermission != null && clubPermission.UserRole == "Admin")
            {
                return 0;
            }
            var subscriptionId = clubPermission?.SubscriptionId;


            if (subscriptionId == null)
            {
                return e.PublicPrice;
            }
            else
            {
                var ep = await _context.EventPrices.FindAsync(e.EventId, subscriptionId.Value);
                var price = ep?.Price ?? e.PublicPrice;
                return price;
            }
        }
    }
}
