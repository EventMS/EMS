using AutoMapper;
using EMS.ClubMember_Services.API.Context;
using EMS.ClubMember_Services.API.Context.Model;
using EMS.Events;
using Serilog;
using TemplateWebHost.Customization.BasicConsumers;

namespace EMS.ClubMember_Services.API.Events
{
    public class ClubSubscriptionCreatedEventConsumer :
            BasicDuplicateConsumer<ClubMemberContext, ClubSubscription, ClubSubscriptionCreatedEvent>
    {
        public ClubSubscriptionCreatedEventConsumer(ClubMemberContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}


