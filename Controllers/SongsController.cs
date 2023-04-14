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
            //var ventaMusicalContext = _context.Songs.Include(s => s.Albume).Include(s => s.Author).Include(s => s.Gender);
            //return View(await ventaMusicalContext.ToListAsync());

			var song = await _context.Songs.Where(x => x.SongState == true).ToListAsync();
			return song != null ?
						View(song) :
						Problem("Entity set 'VentaMusicalContext.Genders'  is null.");
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
			return View();
		}

		// POST: Songs/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SongId,SongName,AuthorId,GenderId,SongYear,SongPrice,SongState,AlbumeId")] Song song)
        {
			if (ModelState.IsValid)
			{


				var queryDescription = (from a in _context.Songs
										where a.SongName.ToUpper() == song.SongName.ToUpper()
										select a).FirstOrDefault();
				if (queryDescription != null)
				{
					ModelState.AddModelError("SongName", "Ya existe una canción con ese nombre");
					return View(song);
				}
				else
				{
					song.SongState = true;
					_context.Add(song);
					await _context.SaveChangesAsync();
				}

			}
			return RedirectToAction("Index", "Songs");
			//if (ModelState.IsValid)
			//{
			//    _context.Add(song);
			//    await _context.SaveChangesAsync();
			//    return RedirectToAction(nameof(Index));
			//}
			//ViewData["AlbumeId"] = new SelectList(_context.Albumes, "AlbumeId", "AlbumeId", song.AlbumeId);
			//ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", song.AuthorId);
			//ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "GenderId", song.GenderId);
			//return View(song);
		}

        // GET: Songs/Edit/5
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
			return View(song);
			//if (id == null || _context.Songs == null)
			//{
			//    return NotFound();
			//}

			//var song = await _context.Songs.FindAsync(id);
			//if (song == null)
			//{
			//    return NotFound();
			//}
			//ViewData["AlbumeId"] = new SelectList(_context.Albumes, "AlbumeId", "AlbumeId", song.AlbumeId);
			//ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", song.AuthorId);
			//ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "GenderId", song.GenderId);
			//return View(song);
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

			if (ModelState.IsValid)
			{
				try
				{
					var query = (from a in _context.Songs
								 where a.SongId == song.SongId
								 select a).FirstOrDefault();


					//actualizar
					query.SongName = song.SongName;
					_context.Update(query);

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
			return View(song);
		}

        // GET: Songs/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Songs == null)
            {
                return Problem("Entity set 'VentaMusicalContext.Songs'  is null.");
            }
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongExists(int id)
        {
          return (_context.Songs?.Any(e => e.SongId == id)).GetValueOrDefault();
        }
    }
}
