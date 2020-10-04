using Microsoft.AspNetCore.Mvc;

namespace EMS.Identity_Services.API.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return new RedirectResult("~/playground");
        }
    }
}
