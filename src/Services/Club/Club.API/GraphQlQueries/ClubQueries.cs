using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Club_Service.API.Context;
using EMS.Club_Service.API.Context.Model;
using EMS.Club_Service_Services.API;
using Microsoft.EntityFrameworkCore;

namespace EMS.Club_Service.API.GraphQlQueries
{
    /// <summary>
    /// Club queries
    /// </summary>
    public class ClubQueries
    {
        private readonly ClubContext _context;
        public ClubQueries(ClubContext context)
        {
            _context = context;
        }

        public IQueryable<Club> Clubs => _context.Clubs.AsQueryable();

        public IQueryable<Club> MyAdminClubs([CurrentUserGlobalState] CurrentUser currentUser)
        {
            return _context.Clubs.Where(club => club.AdminId == currentUser.UserId).AsQueryable();
        }

        public async Task<Club> ClubByID(Guid clubId) => await _context.Clubs.FindAsync(clubId);

        public async Task<Club> ClubByName(string name) => await _context.Clubs.FirstOrDefaultAsync(club => club.Name == name);

    }
}
