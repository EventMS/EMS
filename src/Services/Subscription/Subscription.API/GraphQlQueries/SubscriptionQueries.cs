using System.Linq;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Subscription.API.Context;

namespace Subscription.API.GraphQlQueries
{
    public class SubscriptionQueries
    {
        private readonly SubscriptionContext _context;
        public SubscriptionQueries(SubscriptionContext context)
        {
            _context = context;
        }

        public IQueryable<Context.ClubSubscription> ClubSubscriptions => _context.ClubSubscriptions.AsQueryable();

        public IQueryable<Context.Club> Clubs => _context.Clubs.AsQueryable();
    }
}
