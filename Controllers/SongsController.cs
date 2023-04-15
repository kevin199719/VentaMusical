using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VentaMusical.Data;
using VentaMusical.Models.Entities;

namespace VentaMusical.Controllers
{
    public class SongsController : Controller
    {
        private readonly VentaMusicalContext _context;

        public SongsController(VentaMusicalContext context)
        {
            _context = context;
        }

        // GET: Songs
        public async Task<IActionResult> Index()
        {
            var songs = await _context.Songs.Include(s => s.Author).Include(s => s.Albume).Where(x => x.SongState == true).ToListAsync();
            return View(songs);
        }

        // GET: Songs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Songs == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Albume)
                .Include(s => s.Author)
                .Include(s => s.Gender)
                .FirstOrDefaultAsync(m => m.SongId == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // GET: Songs/Create
        public IActionResult Create()
        {
            ViewBag.Authors = new SelectList(_context.Authors, "AuthorId", "AuthorName");
            ViewBag.Genders = new SelectList(_context.Genders, "GenderId", "GenderDescription");
            ViewBag.Albums = new SelectList(_context.Albumes, "AlbumeId", "AlbumeName");
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SongId,SongName,AuthorId,GenderId,SongYear,SongPrice,SongState,AlbumeId")] Song song)
        {

            song.SongState = true;
            _context.Add(song);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Songs == null)
            {
                return NotFound();
            }


            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            ViewBag.Authors = new SelectList(_context.Authors, "AuthorId", "AuthorName");
            ViewBag.Genders = new SelectList(_context.Genders, "GenderId", "GenderDescription");
            ViewBag.Albums = new SelectList(_context.Albumes, "AlbumeId", "AlbumeName");

            return View(song);
        }


        // POST: Songs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SongId,SongName,AuthorId,GenderId,SongYear,SongPrice,SongState,AlbumeId")] Song song)
        {
            if (id != song.SongId)
            {
                return NotFound();
            }

            song.SongState = true; // asignar el valor por defecto



            try
            {
                song.SongState = true;
                _context.Update(song);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(song.SongId))
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

        // GET: Songs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Songs == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .FirstOrDefaultAsync(m => m.SongId == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Songs == null)
            {

                return Problem("Entity set 'VentaMusicalContext.Albumes'  is null.");
            }
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {

                song.SongState = false;
                _context.Songs.Update(song);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongExists(int id)
        {
            return _context.Songs.Any(e => e.SongId == id);
        }
    }
}