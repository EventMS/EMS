using System;
using System.Collections.Generic;
using Microsoft.Azure.Amqp.Serialization;

namespace EMS.ClubMember_Services.API.Context.Model
{
    public class ClubSubscription
    {
        public Guid ClubSubscriptionId { get; set; }
        public Guid ClubId { get; set; }

        public List<ClubMember> ClubMembers { get; set; }
    }
}