using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMS.Club_Service.API.Controllers.Request
{
    /// <summary>
    /// Create club request
    /// </summary>
    public class CreateClubRequest
    {       
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(8)]
        public string PhoneNumber { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(4)]
        public string RegistrationNumber { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(8)]
        public string AccountNumber { get; set; }
        [Required]
        [MaxLength(50)]
        public string Address { get; set; }
        public List<String> Locations { get; set; }
    }
}
