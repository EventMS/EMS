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

namespace Permission.API.IntegrationEvents
{
    public class UserCreatedEventPermissionConsumer :
            IConsumer<UserCreatedIntegrationEvent>
    {
        private readonly PermissionContext _permissionContext;

        public UserCreatedEventPermissionConsumer(PermissionContext permissionContext)
        {
            _permissionContext = permissionContext;
        }

        public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
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

    public class ClubCreatedIntegrationEventPermissionConsumer :
        IConsumer<ClubCreatedIntegrationEvent>
    {
        private readonly PermissionContext _permissionContext;

        public ClubCreatedIntegrationEventPermissionConsumer(PermissionContext permissionContext)
        {
            _permissionContext = permissionContext;
        }

        public async Task Consume(ConsumeContext<ClubCreatedIntegrationEvent> context)
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
