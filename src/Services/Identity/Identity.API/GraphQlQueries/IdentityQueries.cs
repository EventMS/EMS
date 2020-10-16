using System;
using System.Collections.Generic;
using System.Linq;
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

        public ApplicationUser User(string id) => _context.Users.Find(id);

        public IQueryable<ApplicationUser> UsersById(List<string> ids) => _context.Users.Where(user => ids.Contains(user.Id)).AsQueryable();
    }
}