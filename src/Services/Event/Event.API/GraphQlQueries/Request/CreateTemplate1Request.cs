using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper.Configuration.Annotations;
using EMS.Event_Services.API.Context.Model;
using EMS.TemplateWebHost.Customization.Attributes;

namespace EMS.Event_Services.API.Controllers.Request
{
    public class CreateEventRequest
    {
        [Required]
        public Guid ClubId { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }

        public float? PublicPrice { get; set; }

        [Required]
        [FutureDate]
        public DateTime StartTime { get; set; }

        [Required]
        [FutureDate]
        public DateTime EndTime { get; set; }

        [Required]
        public List<EventPriceRequest> EventPrices { get; set; }
        public List<Guid> Locations { get; set; }
        public List<Guid> InstructorForEvents { get; set; }
    }

    public class EventPriceRequest
    {
        [Required]
        public Guid ClubSubscriptionId { get; set; }
        [Range(0, 10000000)]
        public float Price { get; set; }
    }
}
