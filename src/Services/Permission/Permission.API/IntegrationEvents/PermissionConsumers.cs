using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using MassTransit;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Permission.API.Context;
using Permission.API.Context.Model;
using Serilog;

namespace Permission.API.Events
{
    public class UserCreatedEventPermissionConsumer :
            IConsumer<UserCreatedEvent>
    {
        private readonly PermissionContext _permissionContext;

        public UserCreatedEventPermissionConsumer(PermissionContext permissionContext)
        {
            _permissionContext = permissionContext;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var userId = new Guid(context.Message.UserId);
            var userAlreadyCreated = _permissionContext.UserPermissions.Find(userId);
            if (userAlreadyCreated == null)
            {
                await _permissionContext.UserPermissions.AddAsync(new UserPermission()
                {
                    UserId = userId
                });
                await _permissionContext.SaveChangesAsync();
            }
        }
    }

    public class ClubCreatedEventPermissionConsumer :
        IConsumer<ClubCreatedEvent>
    {
        private readonly PermissionContext _permissionContext;

        public ClubCreatedEventPermissionConsumer(PermissionContext permissionContext)
        {
            _permissionContext = permissionContext;
        }

        public async Task Consume(ConsumeContext<ClubCreatedEvent> context)
        {
            var clubId = context.Message.ClubId;
            var userAlreadyCreated = _permissionContext.ClubAdministratorPermissions.Find(clubId);
            if (userAlreadyCreated == null)
            {
                var clubAdmin = new ClubAdministratorPermission
                {
                    ClubId = clubId,
                    Users = new List<UserAdministratorPermission>()
                    {
                        new UserAdministratorPermission() {ClubId = clubId, UserId = context.Message.AdminId}
                    }
                };

                await _permissionContext.ClubAdministratorPermissions.AddAsync(clubAdmin);
                await _permissionContext.SaveChangesAsync();
            }
        }
    }

}
