using System;
using System.ComponentModel.DataAnnotations;
using TemplateWebHost.Customization.Attributes;

namespace Subscription.API.GraphQlQueries.Request
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
    }
}