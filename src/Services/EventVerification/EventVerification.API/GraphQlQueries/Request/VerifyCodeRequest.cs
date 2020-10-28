
using System;
using System.ComponentModel.DataAnnotations;

namespace EMS.EventVerification_Services.API.Controllers.Request
{
    public class VerifyCodeRequest
    {
        public Guid EventId { get; set; }

        public string Code { get; set; }
    }
}
