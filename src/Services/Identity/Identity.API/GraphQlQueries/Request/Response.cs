using Identity.API.Context.Models;

namespace Identity.API.Controllers
{
    public class Response
    {
        public ApplicationUser User { get; set; }
        public string Token { get; set; }
    }
}