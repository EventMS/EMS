using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
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
            else
            {
                Log.Information("User have already been created with ID: {Id}", context.Message.UserId);
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
            var permissionAlreadyGiven = await _permissionContext.UserAdministratorPermission
                .FirstOrDefaultAsync(user => user.UserId == context.Message.AdminId
                                             && user.ClubId == context.Message.ClubId);
            if (permissionAlreadyGiven != null)
            {
                Log.Information("ClubCreatedEvent have already been completed previously. ");
                return;
            }


            var clubCreated = _permissionContext.ClubAdministratorPermissions.Find(clubId);
            if (clubCreated == null)
            {
                clubCreated = new ClubAdministratorPermission
                {
                    ClubId = clubId,
                    Users = new List<UserAdministratorPermission>()
                    {
                        new UserAdministratorPermission() {ClubId = clubId, UserId = context.Message.AdminId}
                    }
                };

                await _permissionContext.ClubAdministratorPermissions.AddAsync(clubCreated);
                await _permissionContext.SaveChangesAsync();
            }
            else
            {
                _permissionContext.UserAdministratorPermission.Add(new UserAdministratorPermission() { ClubId = clubId, UserId = context.Message.AdminId });
                await _permissionContext.SaveChangesAsync();
            }
        }
    }

}
