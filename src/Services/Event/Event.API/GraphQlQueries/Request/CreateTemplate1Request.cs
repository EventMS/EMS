using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper.Configuration.Annotations;
using EMS.Event_Services.API.Context.Model;

namespace EMS.Event_Services.API.Controllers.Request
{
    public class CreateEventRequest
    {
        [Required]
        public Guid ClubId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public List<SubscriptionEventPriceRequest> SubscriptionEventPrices { get; set; }
        public List<Guid> Locations { get; set; }
        public List<Guid> InstructorForEvents { get; set; }
    }

    public class SubscriptionEventPriceRequest
    {
        public Guid SubscriptionId { get; set; }
        public int Price { get; set; }
    }
}
