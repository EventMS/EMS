using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace EMS.Payment_Services.API.Context.Model
{
    public class User
    {
        public Guid UserId { get; set; }

        public string StripeUserId { get; set; }
    }
}
