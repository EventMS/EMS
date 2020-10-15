using System.Collections.Generic;
using System.Threading.Tasks;
using EMS.Events;
using EMS.Permission_Services.API.Context;
using EMS.Permission_Services.API.Context.Model;
using MassTransit;

namespace EMS.Permission_Services.API.Events
{
    public class ClubCreatedEventConsumer :
        IConsumer<ClubCreatedEvent>
    {
        private readonly PermissionContext _permissionContext;

        public ClubCreatedEventConsumer(PermissionContext permissionContext)
        {
            _permissionContext = permissionContext;
        }

        public async Task Consume(ConsumeContext<ClubCreatedEvent> context)
        {
            var clubId = context.Message.ClubId;
            if (_permissionContext.Clubs.Find(clubId) == null)
            {
                _permissionContext.Clubs.Add(new Club()
                {
                    ClubId = clubId,
                    Users = new List<Role>()
                    {
                        new Role()
                        {
                            UserId = context.Message.AdminId,
                            ClubId = clubId,
                            UserRole = "Admin"
                        }
                    }
                });
                _permissionContext.SaveChanges();
            }
        }
    }
}