using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VentaMusical.Data;
using VentaMusical.Models;

namespace VentaMusical.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSongs()
        {
            List<SongModel> songs = SalesBLL.getSongs();
            return Json(songs);
        }
    }
}
