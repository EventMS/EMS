using System;
using System.Linq;
using EMS.Room_Services.API.Context;
using EMS.Room_Services.API.Context.Model;

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

        public IQueryable<Booking> Bookings => _context.Bookings.AsQueryable();

        public IQueryable<Club> Clubs => _context.Clubs.AsQueryable();
    }
}
