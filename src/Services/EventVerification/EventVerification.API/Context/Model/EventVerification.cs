using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate;

namespace EMS.EventVerification_Services.API.Context.Model
{
    public class EventVerification
    {
        public Guid EventId { get; set; }

        public Guid UserId { get; set; }

        public PresenceStatusEnum Status { get; set; }

        public int EventVerificationId { get; set; }

        public string Code => EventVerificationId.ToString("X4"); //The barebone solution. 

        public EventVerification()
        {
            Status = PresenceStatusEnum.SignedUp;
        }
    }

    public enum PresenceStatusEnum
    {
        SignedUp,
        Attend,
        DidNotAttend
    }
}
