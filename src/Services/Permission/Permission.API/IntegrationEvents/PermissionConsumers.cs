using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
}
