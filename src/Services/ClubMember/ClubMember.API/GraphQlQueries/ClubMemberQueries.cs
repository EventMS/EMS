using System.Linq;
using EMS.ClubMember_Services.API.Context;
using EMS.ClubMember_Services.API.Context.Model;

namespace EMS.ClubMember_Services.API.GraphQlQueries
{
    public class ClubMemberQueries
    {
        private readonly ClubMemberContext _context;
        public ClubMemberQueries(ClubMemberContext context)
        {
            _context = context;
        }

        public IQueryable<Context.Model.ClubMember> ClubMembers => _context.ClubMembers.AsQueryable();
        public IQueryable<ClubSubscription> ClubSubscriptions => _context.ClubSubscriptions.AsQueryable();
    }
}
