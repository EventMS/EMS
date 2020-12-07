using Microsoft.AspNetCore.Mvc;

namespace EMS.Identity_Services.API.Controllers
{
    /// <summary>
    /// Basic home controller that navigates to playground. 
    /// </summary>
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return new RedirectResult("~/playground");
        }
    }
}
