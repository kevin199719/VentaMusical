using Microsoft.AspNetCore.Mvc;

namespace VentaMusical.Controllers
{
    public class ClientsController : Controller
    {
        public IActionResult IndexClients()
        {
            return View();
        }
    }
}
