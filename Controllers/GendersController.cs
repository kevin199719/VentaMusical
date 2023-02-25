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
    public class GendersController : Controller
    {
        private readonly VentaMusicalContext _context;

        public GendersController(VentaMusicalContext context)
        {
            _context = context;
        }

        // GET: Genders
        public async Task<IActionResult> Index()
        {
            var genre = await _context.Genders.Where(x => x.GenderState == true).ToListAsync();
            return genre != null ?
                        View(genre) :
                        Problem("Entity set 'VentaMusicalContext.Genders'  is null.");

         }

        // GET: Genders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Genders == null)
            {
                return NotFound();
            }

            var gender = await _context.Genders
                .FirstOrDefaultAsync(m => m.GenderId == id);
            if (gender == null)
            {
                return NotFound();
            }

            return View(gender);
        }

        // GET: Genders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GenderId,GenderDescription,GenderState")] Gender gender)
        {
            if (ModelState.IsValid)
            {
                
              
                    var queryDescription = (from a in _context.Genders
                                            where a.GenderDescription.ToUpper()== gender.GenderDescription.ToUpper()
                                            select a).FirstOrDefault();
                    if (queryDescription != null)
                    {
                        ModelState.AddModelError("GenderDescription", "Ya existe un genero con ese nombre");
                        return View(gender);
                    }
                    else
                    {
                        gender.GenderState = true;
                        _context.Add(gender);
                        await _context.SaveChangesAsync();
                    }
                    
            }
            return View(gender);
        }

        // GET: Genders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Genders == null)
            {
                return NotFound();
            }

            var gender = await _context.Genders.FindAsync(id);
            if (gender == null)
            {
                return NotFound();
            }
            return View(gender);
        }

        // POST: Genders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GenderId,GenderDescription,GenderState")] Gender gender)
        {
            if (id != gender.GenderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var query = (from a in _context.Genders
                                 where a.GenderId == gender.GenderId
								 select a).FirstOrDefault();
                    // modifica
                    var newGender = new Gender();
                   newGender.GenderDescription = query.GenderDescription;
                    
                    _context.Update(newGender);
                    //actualizar
                    query.GenderDescription = gender.GenderDescription;
                    _context.Update(query);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenderExists(gender.GenderId))
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
            return View(gender);
        }

        // GET: Genders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Genders == null)
            {
                return NotFound();
            }

            var gender = await _context.Genders
                .FirstOrDefaultAsync(m => m.GenderId == id);
            if (gender == null)
            {
                return NotFound();
            }

            return View(gender);
        }

        // POST: Genders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Genders == null)
            {
                return Problem("Entity set 'VentaMusicalContext.Genders'  is null.");
            }
            var gender = await _context.Genders.FindAsync(id);
            if (gender != null)
            {
                gender.GenderState = false;
                _context.Genders.Update(gender);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenderExists(int id)
        {
          return (_context.Genders?.Any(e => e.GenderId == id)).GetValueOrDefault();
        }
    }
}
