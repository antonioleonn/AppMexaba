using Microsoft.AspNetCore.Mvc;

namespace AppMexaba.Controllers
{
    public class KioskoMexabaController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Kiosko Mexaba";
            return View();
        }
    }
}
