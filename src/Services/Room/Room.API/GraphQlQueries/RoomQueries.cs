using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Room_Services.API.Context;
using EMS.Room_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;

namespace EMS.Room_Services.API.GraphQlQueries
{
    public class RoomQueries
    {
        private readonly RoomContext _context;
        public RoomQueries(RoomContext context)
        {
            _context = context;
        }

        public IQueryable<Room> Rooms => _context.Rooms.AsQueryable();

        public IQueryable<Room> RoomsForClub(Guid clubId) => _context.Rooms.Where(room => room.ClubId == clubId).AsQueryable();

        public IQueryable<Booking> BookingsForClub(Guid clubId) => _context.Rooms
            .Include(r => r.Bookings)
            .Where(room => room.ClubId == clubId).SelectMany(r => r.Bookings).AsQueryable();

        public async Task<Room> RoomById(Guid roomId) => await _context.Rooms.FirstOrDefaultAsync(room => room.RoomId == roomId);

        public IQueryable<Booking> Bookings => _context.Bookings.AsQueryable();

        public IQueryable<Club> Clubs => _context.Clubs.AsQueryable();
    }
}
