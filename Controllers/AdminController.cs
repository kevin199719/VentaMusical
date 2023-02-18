using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VentaMusical.Data;
using VentaMusical.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace VentaMusical.Controllers
{
    public class AdminController : Controller  
    {
        private readonly VentaMusicalContext _context;

        public AdminController(VentaMusicalContext context)
        {
            _context = context;
        }

        // GET: Users2
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Where(x => x.UserState == true).ToListAsync();
            return users != null ?
                        View(users) :
                        Problem("Entity set 'VentaMusicalContext.Users'  is null.");
        }

        // GET: Users2/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users2/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserIdentification,UserName,UserGender,UserEmail,UserAlias")] User user,int Module, string Password)
        {
            if (ModelState.IsValid)
            {
                //Comprobar si existe pero esta de baja
                //Comparar cedula para ver si existe
                var queryIdentification = (from a in _context.Users
                             where a.UserIdentification == user.UserIdentification
                             select a).FirstOrDefault();
                if (queryIdentification != null)
                {
                    queryIdentification.UserState = true;
                    _context.Users.Update(queryIdentification);
                    //Guardar Cambios
                    await _context.SaveChangesAsync();
                }
                else {
                    //añadir usuario
                    user.UserState = true;
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    //Traerse Id del usuario creado para ligarle roles y contraseña
                    var query = (from a in _context.Users
                                 where a.UserId == user.UserId
                                 select a).FirstOrDefault();

                    //Añadir Perfil
                    var userProfile = new Profile();
                    userProfile.UserId = query.UserId;
                    userProfile.ModuleId = Module;
                    _context.Add(userProfile);

                    //Añadir Contraseña
                    var userPass = new UserPassword();
                    userPass.UserId = query.UserId;
                    userPass.Password = Password;
                    _context.Add(userPass);

                    //Guardar Cambios
                    await _context.SaveChangesAsync();
                }

               

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users2/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserIdentification,UserName,UserGender,UserEmail,UserAlias")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: Users2/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'VentaMusicalContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.UserState = false;
                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
