
using System.ComponentModel.DataAnnotations;

namespace Subscription.API.GraphQlQueries.Request
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
