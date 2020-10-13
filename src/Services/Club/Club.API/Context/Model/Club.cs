using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace EMS.Club_Service.API.Context.Model
{
    public class Club
    {
        public Guid ClubId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Address { get; set; }
        public Guid AdminId { get; set; } 
        public HashSet<Guid> InstructorIds { get; set; }
    }
}
