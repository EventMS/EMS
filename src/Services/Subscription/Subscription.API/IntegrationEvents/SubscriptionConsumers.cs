using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using MassTransit;
using EMS.Subscription_Services.API.Context;
using TemplateWebHost.Customization.BasicConsumers;

namespace EMS.Subscription_Services.API.IntegrationEvents
{
    public class ClubCreatedEventConsumer :
            BasicDuplicateConsumer<SubscriptionContext, Club, ClubCreatedEvent>
    {
        public ClubCreatedEventConsumer(SubscriptionContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
