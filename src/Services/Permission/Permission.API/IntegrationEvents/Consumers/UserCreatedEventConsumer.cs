using System;
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
            var userId = context.Message.UserId;
            var userAlreadyCreated = _permissionContext.Users.Find(userId);
            if (userAlreadyCreated == null)
            {
                await _permissionContext.Users.AddAsync(new User()
                {
                    UserId = userId,
                });
                await _permissionContext.SaveChangesAsync();
            }
        }
    }
}
