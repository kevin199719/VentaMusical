using Microsoft.AspNetCore.Mvc;

namespace VentaMusical.Controllers
{
    public class SongsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
