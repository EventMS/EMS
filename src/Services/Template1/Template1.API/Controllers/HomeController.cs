using Microsoft.AspNetCore.Mvc;

namespace Template1.API.Controllers
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
