﻿using System;
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
            var albumes = await _context.Albumes.Where(x => x.AlbumeState == true).ToListAsync();
            return albumes != null ?
                        View(albumes) :
                        Problem("Entity set 'VentaMusicalContext.Users'  is null.");
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId");
            return View();
        }

        // POST: Albumes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlbumeId,AuthorId,AlbumeName,AlbumeYear,AlbumeState")] Albume albume)
        {
            if (ModelState.IsValid)
            {
                var queryAlbumId = (from a in _context.Albumes
                                    where a.AlbumeId == albume.AlbumeId
                                    select a).FirstOrDefault();

                if (queryAlbumId != null )
                {
                    ModelState.AddModelError("AlbumId", "Ya existe un Album con este nombre");
                    return View(albume);
                } else
                {
                    albume.AlbumeState = true;
                    _context.Add(albume);
                    await _context.SaveChangesAsync();
                }
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", albume.AuthorId);
            return View(albume);
        }

        // GET: Albumes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Albumes == null)
            {
                return NotFound();
            }

            var albume = await _context.Albumes.FindAsync(id);
            if (albume == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", albume.AuthorId);
            return View(albume);
        }

        // POST: Albumes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlbumeId,AuthorId,AlbumeName,AlbumeYear,AlbumeState")] Albume albume)
        {
            if (id != albume.AlbumeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(albume);
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
          return (_context.Albumes?.Any(e => e.AlbumeId == id)).GetValueOrDefault();
        }
    }
}