using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EMS.Room_Services.API.Context.EntityConfigurations;
using EMS.TemplateWebHost.Customization.Context;

namespace EMS.Room_Services.API.Context
{
    using Model;
    public class RoomContext : BaseContext
    {
        public RoomContext(DbContextOptions<RoomContext> options) : base(options)
        {
        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Club> Clubs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RoomEntityTypeConfiguration());
            builder.ApplyConfiguration(new BookingEntityTypeConfiguration());
            builder.ApplyConfiguration(new ClubEntityTypeConfiguration());
        }
    }


    public class RoomContextDesignFactory : IDesignTimeDbContextFactory<RoomContext>
    {
        public RoomContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<RoomContext>()
                .UseSqlServer("Server=.;Initial Catalog=Services.RoomDb;Integrated Security=true");

            return new RoomContext(optionsBuilder.Options);
        }
    }
}
