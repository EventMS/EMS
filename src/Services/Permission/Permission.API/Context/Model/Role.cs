using System;

namespace EMS.Permission_Services.API.Context.Model
{
    public class Role
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ClubId { get; set; }
        public Club Club { get; set; }
        public string UserRole { get; set; }
    }
}