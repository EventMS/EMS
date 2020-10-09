using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EMS.TemplateWebHost.Customization.Attributes;

namespace EMS.Subscription_Services.API.GraphQlQueries.Request
{
    public class CreateClubSubscriptionRequest
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        [Required]
        [Range(0,1000000)]
        public int Price { get; set; }

        [Required]
        [NotEmpty]
        public Guid ClubId { get; set; }

        public Guid ReferenceId { get; set; }
    }

    public class EventPrice{
        [Required]
        [NotEmpty]
        public Guid EventId { get; set; }

        [Required]
        [Range(0,1000000)]
        public float Price { get; set; }
    }
}
