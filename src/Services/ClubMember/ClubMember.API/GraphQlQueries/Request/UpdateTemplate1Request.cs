
using System;
using System.ComponentModel.DataAnnotations;
using EMS.TemplateWebHost.Customization.Attributes;

namespace EMS.ClubMember_Services.API.Controllers.Request
{
    public class UpdateClubMemberRequest
    {
        [Required]
        public string NameOfSubscription { get; set; }

        [Required]
        [NotEmpty]
        public Guid UserId { get; set; }

        [Required]
        [NotEmpty]
        public Guid ClubId { get; set; }
    }
}
