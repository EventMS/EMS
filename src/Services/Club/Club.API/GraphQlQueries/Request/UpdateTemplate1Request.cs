
using System;

namespace Club.API.Controllers.Request
{
    public class UpdateClubRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Address { get; set; }
    }
}
