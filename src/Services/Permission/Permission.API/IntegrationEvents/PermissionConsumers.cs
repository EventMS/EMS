using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EMS.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using EMS.Permission_Services.API.Context;
using EMS.Permission_Services.API.Context.Model;
using Serilog;

namespace EMS.Permission_Services.API.Events
{
    public class UserCreatedEventConsumer :
            IConsumer<UserCreatedEvent>
    {
        private readonly PermissionContext _permissionContext;

        public UserCreatedEventConsumer(PermissionContext permissionContext)
        {
            _permissionContext = permissionContext;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var userId = new Guid(context.Message.UserId);
            var userAlreadyCreated = _permissionContext.Users.Find(userId);
            if (userAlreadyCreated == null)
            {
                await _permissionContext.Users.AddAsync(new User()
                {
                    UserId = userId
                });
                await _permissionContext.SaveChangesAsync();
            }
        }
    }

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
