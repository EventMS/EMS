using System.Threading.Tasks;
using AutoMapper;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Context.Model;
using EMS.Events;
using MassTransit;
using TemplateWebHost.Customization.BasicConsumers;

namespace EMS.Event_Services.API.Events
{
    //Ensure mappers are on point. 
    public class InstructorAddedEventConsumer :
        BasicDuplicateConsumer<EventContext, Instructor, InstructorAddedEvent>
    {
        public InstructorAddedEventConsumer(EventContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}