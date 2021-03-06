using System;
using System.ComponentModel.DataAnnotations;

namespace EMS.Identity_Services.API.GraphQlQueries
{
    public class CreateUserRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
}