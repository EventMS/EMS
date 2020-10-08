using System.Threading.Tasks;
using AutoMapper;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Context.Model;
using EMS.Events;
using MassTransit;
using TemplateWebHost.Customization.BasicConsumers;

namespace EMS.Event_Services.API.Events
{

    public class ClubSubscriptionCreatedEventConsumer :
        BasicDuplicateConsumer<EventContext, ClubSubscription, ClubSubscriptionCreatedEvent>
    {
        public ClubSubscriptionCreatedEventConsumer(EventContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}