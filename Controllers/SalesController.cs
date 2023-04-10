using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using VentaMusical.Data;
using VentaMusical.Models;
using VentaMusical.Models.Entities;

namespace VentaMusical.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Car()
        {
            return View();
        }

        public JsonResult GetSongs(string SongName, string AlbumeName, string AuthorName, string GenderDescription)
        {
            List<SongModel> songs = SalesBLL.getSongs(SongName,  AlbumeName,  AuthorName,  GenderDescription);
            return Json(songs);
        }
      
        public int AddSong(int songId)
        {
            int userId = HttpContext.Session.GetInt32("_Id") ?? 0;
            int invoiceHeaderId = SalesBLL.AddSongToInvoice(userId, songId);

            return invoiceHeaderId;
        }

        public JsonResult getInvoiceSongs()
        {
            int userId = HttpContext.Session.GetInt32("_Id") ?? 0;
            List<SongModel> songs = SalesBLL.getInvoiceSongs(userId);
            return Json(songs);
        }

        public JsonResult getUserCard()
        {
            int userId = HttpContext.Session.GetInt32("_Id") ?? 0;
            UserCard card = SalesBLL.getUserCard(userId);
            return Json(card);
        }
    }
}
