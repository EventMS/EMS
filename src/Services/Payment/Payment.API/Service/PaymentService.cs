using System;
using System.Threading.Tasks;
using EMS.Club_Service_Services.API;
using EMS.Payment_Services.API.Context;
using EMS.TemplateWebHost.Customization;

namespace EMS.Payment_Services.API.GraphQlQueries
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentContext _context;

        public PaymentService(PaymentContext context)
        {
            _context = context;
        }

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