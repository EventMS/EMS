
using System.ComponentModel.DataAnnotations;

namespace EMS.Subscription_Services.API.GraphQlQueries.Request
{

    public class UpdateClubSubscriptionRequest
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [Range(0, 1000000)]
        public int Price { get; set; }
    }
}
