using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using EMS.Permission_Services.API.Context;
using EMS.Permission_Services.API.Context.Model;
using MassTransit;

namespace EMS.Permission_Services.API.Events
{
    public class ClubMemberCreatedEventConsumer :
        IConsumer<ClubMemberCreatedEvent>
    {
        private readonly PermissionContext _permissionContext;

        public ClubMemberCreatedEventConsumer(PermissionContext permissionContext)
        {
            _permissionContext = permissionContext;
        }

        public async Task Consume(ConsumeContext<ClubMemberCreatedEvent> context)
        {

            var role = _permissionContext.Roles.SingleOrDefault(r =>
                r.ClubId == context.Message.ClubId && r.UserId == context.Message.UserId);
            if (role == null)
            {
                _permissionContext.Roles.Add(new Role()
                {
                    ClubId = context.Message.ClubId,
                    UserId = context.Message.UserId,
                    ClubSubscriptionId = context.Message.ClubSubscriptionId,
                    UserRole = "Member"
                });
                await _permissionContext.SaveChangesAsync();
            }
            else if (role.UserRole == "Admin")
            {
                role.ClubSubscriptionId = context.Message.ClubSubscriptionId;
                _permissionContext.Roles.Update(role);
                await _permissionContext.SaveChangesAsync();
            }
        }
    }
}