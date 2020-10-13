using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using EMS.Permission_Services.API.Context;
using MassTransit;

namespace EMS.Permission_Services.API.Events
{
    public class ClubMemberUpdatedEventConsumer :
        IConsumer<ClubMemberUpdatedEvent>
    {
        private readonly PermissionContext _permissionContext;

        public ClubMemberUpdatedEventConsumer(PermissionContext permissionContext)
        {
            _permissionContext = permissionContext;
        }

        public async Task Consume(ConsumeContext<ClubMemberUpdatedEvent> context)
        {

            var role = _permissionContext.Roles.SingleOrDefault(r =>
                r.ClubId == context.Message.ClubId && r.UserId == context.Message.UserId);
            if (role != null)
            {
                role.SubscriptionId = context.Message.ClubSubscriptionId;
                _permissionContext.Roles.Update(role);
                await _permissionContext.SaveChangesAsync();
            }
        }
    }
}