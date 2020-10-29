using System;
using System.Collections.Generic;
using System.Linq;
using EMS.Club_Service_Services.API;
using EMS.Identity_Services.API.Context.Models;
using EMS.Identity_Services.API.Data;

namespace EMS.Identity_Services.API.GraphQlQueries
{
    public class IdentityQueries
    {
        private readonly ApplicationDbContext _context;
        public IdentityQueries(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<ApplicationUser> Users => _context.Users.AsQueryable();

        public ApplicationUser User(Guid id) => _context.Users.Find(id.ToString());

        public ApplicationUser CurrentUser([CurrentUserGlobalState] CurrentUser user) => _context.Users.Find(user.UserId.ToString());

        public IQueryable<ApplicationUser> UsersById(List<string> ids) => _context.Users.Where(user => ids.Contains(user.Id)).AsQueryable();
    }
}