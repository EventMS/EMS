using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace EMS.ClubMember_Services.API.Context.Model
{
    public class ClubMember
    {
        public Guid UserId { get; set; }
        public Guid ClubId { get; set; }
        public Guid ClubSubscriptionId { get; set; }

        public ClubMember() { }
    }
}
