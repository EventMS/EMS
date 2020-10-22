using System.Linq;
using EMS.Payment_Services.API.Context;
using EMS.Payment_Services.API.Context.Model;

namespace EMS.Payment_Services.API.GraphQlQueries
{
    public class PaymentQueries
    {
        private readonly PaymentContext _context;
        public PaymentQueries(PaymentContext context)
        {
            _context = context;
        }

        public IQueryable<User> Users => _context.Users.AsQueryable();

        public IQueryable<ClubSubscription> ClubSubscriptions => _context.ClubSubscriptions.AsQueryable();
    }
}
