using System;
using System.Collections.Generic;
using Microsoft.Azure.Amqp.Serialization;

namespace EMS.ClubMember_Services.API.Context.Model
{
    public class ClubSubscription
    {
        public Guid ClubId { get; set; }
        public string NameOfSubscription { get; set; }

        public List<ClubMember> ClubMembers { get; set; }
    }
}