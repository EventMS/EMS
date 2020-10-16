
using EMS.TemplateWebHost.Customization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMS.Event_Services.API.Controllers.Request
{
    public class UpdateEventRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [FutureDate]
        public DateTime StartTime { get; set; }

        public float? PublicPrice { get; set; }

        [Required]
        [FutureDate]
        public DateTime EndTime { get; set; }

        [Required]
        public List<EventPriceRequest> EventPrices { get; set; }
        public List<Guid> Locations { get; set; }
        public List<Guid> InstructorForEvents { get; set; }
    }
}
