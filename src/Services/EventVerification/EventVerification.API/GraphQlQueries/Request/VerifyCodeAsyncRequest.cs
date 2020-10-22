
using System;
using System.ComponentModel.DataAnnotations;

namespace EMS.EventVerification_Services.API.Controllers.Request
{
    public class VerifyCodeAsyncRequest
    {
        public Guid EventId { get; set; }

        public string Code { get; set; }
    }
}
