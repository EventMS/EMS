using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Club_Service_Services.API;
using EMS.Payment_Services.API.Context;
using EMS.Payment_Services.API.Context.Model;

namespace EMS.Payment_Services.API.GraphQlQueries
{
    public class PaymentQueries
    {
        private readonly PaymentContext _context;
        private readonly IPaymentService _service;
        public PaymentQueries(PaymentContext context, IPaymentService service)
        {
            _context = context;
            _service = service;
        }

        public IQueryable<User> Users => _context.Users.AsQueryable();

        public IQueryable<ClubSubscription> ClubSubscriptions => _context.ClubSubscriptions.AsQueryable();


        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<float?> EventUserPrice(Guid eventId, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            return await _service.CalculateEventPriceForUserAsync(eventId, currentUser);
        }
    }
}
