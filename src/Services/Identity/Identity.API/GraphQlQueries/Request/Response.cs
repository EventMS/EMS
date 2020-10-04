using EMS.Identity_Services.API.Context.Models;

namespace EMS.Identity_Services.API.Controllers
{
    public class Response
    {
        public ApplicationUser User { get; set; }
        public string Token { get; set; }
    }
}