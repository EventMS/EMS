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
        public Context.Model.Club Club(string name) => _context.Clubs.First(club => club.Name == name);

        public Club ClubByID(Guid clubId) => _context.Clubs.Find(clubId);

        public async Task<Club> ClubByName(string name) => await _context.Clubs.FirstOrDefaultAsync(club => club.Name == name);

    }
}
