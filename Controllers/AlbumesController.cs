using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VentaMusical.Data;
using VentaMusical.Models;
using VentaMusical.Models.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VentaMusical.Controllers
{
    public class AlbumesController : Controller
    {
        private readonly VentaMusicalContext _context;

        public AlbumesController(VentaMusicalContext context)
        {
            _context = context;
        }

        // GET: Albumes
        public async Task<IActionResult> Index()
        {
            //var albumes = await _context.Albumes.Where(x => x.AlbumeState == true).ToListAsync();
            //return albumes != null ?
            //            View(albumes) :
            //            Problem("Entity set 'VentaMusicalContext.Users'  is null.");
            var albums = _context.Albumes
                .Include(a => a.Author).Where(a => a.AlbumeState == true)
                .ToList();
            return View(albums);
        }

        // GET: Albumes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Albumes == null)
            {
                return NotFound();
            }

            var albume = await _context.Albumes
                .Include(a => a.Author)
                .FirstOrDefaultAsync(m => m.AlbumeId == id);
            if (albume == null)
            {
                return NotFound();
            }

            return View(albume);
        }

        // GET: Albumes/Create
        public IActionResult Create()
        {
            ViewBag.Authors = new SelectList(_context.Authors, "AuthorId", "AuthorName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,AlbumeName,AlbumeYear")] Albume albume)
        {
            Albume alm = new Albume();
            alm.AlbumeState = true;
            alm.AuthorId = albume.AuthorId;
            alm.AlbumeName = albume.AlbumeName;
            alm.AlbumeYear =albume.AlbumeYear;
            
            _context.Add(alm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


            //ViewBag.Authors = new SelectList(_context.Authors, "AuthorId", "AuthorName", albume.AuthorId);
            return View(albume);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var albume = await _context.Albumes.FindAsync(id);
            if (albume == null)
            {
                return NotFound();
            }

            var authors = await _context.Authors.ToListAsync();
            ViewBag.Authors = new SelectList(authors, "AuthorId", "AuthorName", albume.AuthorId);

            return View(albume);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlbumeId,AuthorId,AlbumeName,AlbumeYear,AlbumeState")] AlbumeEdit albume)
        {
            if (id != albume.AlbumeId)
            {
                return NotFound();
            }

            albume.AlbumeState = true; // asignar el valor por defecto

            if (ModelState.IsValid)
            {
                try
                {
                    //Traerse Id del usuario creado para Modificar Perfil
                    var query = (from a in _context.Albumes
                                 where a.AlbumeId == albume.AlbumeId
                                 select a).FirstOrDefault();



                    // Actualizar datos del usuario editados
                    query.AuthorId = albume.AuthorId;
                    query.AlbumeName = albume.AlbumeName;
                    query.AlbumeYear = albume.AlbumeYear;
                    query.AlbumeState = albume.AlbumeState;
                    _context.Update(query);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumeExists(albume.AlbumeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", albume.AuthorId);
            return View(albume);
        }

        // GET: Albumes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Albumes == null)
            {
                return NotFound();
            }

            var albume = await _context.Albumes
                .Include(a => a.Author)
                .FirstOrDefaultAsync(m => m.AlbumeId == id);
            if (albume == null)
            {
                return NotFound();
            }

            return View(albume);
        }

        // POST: Albumes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Albumes == null)
            {

                return Problem("Entity set 'VentaMusicalContext.Albumes'  is null.");
            }
            var albume = await _context.Albumes.FindAsync(id);
            if (albume != null)
            {

                albume.AlbumeState = false;
                _context.Albumes.Update(albume);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumeExists(int id)
        {
            return _context.Albumes.Any(e => e.AlbumeId == id);
        }
    }
}
