using Microsoft.eShopOnContainers.Services.Identity.API.Models;

namespace Identity.API.Controllers
{
    public class Response
    {
        public ApplicationUser User { get; set; }
        public string Token { get; set; }
    }
}