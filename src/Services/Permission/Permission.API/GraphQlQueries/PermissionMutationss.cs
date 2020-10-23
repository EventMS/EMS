using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Club_Service_Services.API;
using EMS.Events;
using EMS.Permission_Services.API.Context;
using EMS.Permission_Services.API.Context.Model;
using EMS.TemplateWebHost.Customization;
using EMS.TemplateWebHost.Customization.EventService;
using EMS.TemplateWebHost.Customization.StartUp;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace EMS.Permission_Services.API.GraphQlQueries
{
    public class PermissionMutationss : BaseMutations
    {
        private readonly PermissionContext _context;
        private readonly IEventService _eventService;

        public PermissionMutationss(IAuthorizationService authorizationService, PermissionContext context, IEventService eventService) : base(authorizationService)
        {
            _context = context;
            _eventService = eventService;
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<Role> AddInstructorAsync(Guid clubId, Guid instructorId)
        {
            await IsAdminIn(clubId);
            var role = await _context.Roles.FindOrThrowAsync(instructorId,clubId);
            if (role.ClubSubscriptionId == null)
            {
                throw new QueryException("User is not member in club");
            }

            if (role.UserRole == "Member")
            {
                role.UserRole = "Instructor";
                _context.Roles.Update(role);
                var @event = new InstructorAddedEvent()
                {
                    ClubId = clubId,
                    UserId = instructorId
                };
                await _eventService.SaveEventAndDbContextChangesAsync(@event);
                await _eventService.PublishEventAsync(@event);
            }

            return role;
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<Role> RemoveInstructorAsync(Guid clubId, Guid instructorId)
        {
            await IsAdminIn(clubId);
            var role = await _context.Roles.FindOrThrowAsync(instructorId, clubId);
            if (role.UserRole == "Instructor")
            {
                role.UserRole = "Member";
                _context.Roles.Update(role);
                var @event = new InstructorDeletedEvent()
                {
                    ClubId = clubId,
                    UserId = instructorId
                };
                await _eventService.SaveEventAndDbContextChangesAsync(@event);
                await _eventService.PublishEventAsync(@event);
            }

            return role;
        }
    }
}