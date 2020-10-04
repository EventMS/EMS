using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Club_Service.API.Context;
using EMS.Club_Service_Services.API;
using Microsoft.EntityFrameworkCore;

namespace EMS.Club_Service.API.GraphQlQueries
{
    public class ClubQueries
    {
        private readonly ClubContext _context;
        public ClubQueries(ClubContext context)
        {
            _context = context;
        }

        public IQueryable<Context.Model.Club> Clubs => _context.Clubs.AsQueryable();

        public async Task<IEnumerable<Context.Model.Club>> MyClubs([CurrentUserGlobalState] CurrentUser currentUser)
        {
            return await _context.Clubs.Where(club => club.AdminId == currentUser.UserId).ToArrayAsync();
        }
    }
}
