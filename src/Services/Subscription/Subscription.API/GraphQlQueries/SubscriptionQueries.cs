using System;
using System.Linq;
using EMS.Subscription_Services.API.Context;

namespace EMS.Subscription_Services.API.GraphQlQueries
{
    public class SubscriptionQueries
    {
        private readonly SubscriptionContext _context;
        public SubscriptionQueries(SubscriptionContext context)
        {
            _context = context;
        }

        public IQueryable<ClubSubscription> ClubSubscriptions => _context.ClubSubscriptions.AsQueryable();

        public IQueryable<ClubSubscription> SubscriptionsForClub(Guid clubId) => _context.ClubSubscriptions
            .Where(clubSub => clubSub.ClubId == clubId).AsQueryable();

        public IQueryable<Club> Clubs => _context.Clubs.AsQueryable();
    }
}
