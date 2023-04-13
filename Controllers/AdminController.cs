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
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Where(x => x.UserState == true).ToListAsync();
            return users != null ?
                        View(users) :
                        Problem("Entity set 'VentaMusicalContext.Users'  is null.");
        }

        // GET: Users2/Details/5
        [Authorize(Roles = "1")]
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
        [Authorize(Roles = "1")]
        public IActionResult Create()
        {
            return View();
        }

        
        public IActionResult CreateClient()
        {
            return View();
        }

        // POST: Users2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserIdentification,UserName,UserGender,UserEmail,UserAlias")] User user, int Module)
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
                else
                {

                    //Comprobar si ya un usuario tiene el mismo "Alias"
                    var queryAlias = (from a in _context.Users
                                      where a.UserAlias.ToUpper() == user.UserAlias.ToUpper()
                                      select a).FirstOrDefault();
                    if (queryAlias != null)
                    {
                        ModelState.AddModelError("UserAlias", "Ya existe un usuario con ese alias.");
                        return View(user);
                    }
                    else
                    {
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
                        var userPassGenerate = GenerateRandomPassword();
                        var userPass = new UserPassword();
                        userPass.UserId = query.UserId;
                        userPass.Password = userPassGenerate;
                        _context.Add(userPass);

                        //Guardar Cambios
                        await _context.SaveChangesAsync();
                    }

                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClient([Bind("UserId,UserIdentification,UserName,UserGender,UserEmail,UserAlias")] User user, int Module)
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
                else
                {

                    //Comprobar si ya un usuario tiene el mismo "Alias"
                    var queryAlias = (from a in _context.Users
                                      where a.UserAlias.ToUpper() == user.UserAlias.ToUpper()
                                      select a).FirstOrDefault();
                    if (queryAlias != null)
                    {
                        ModelState.AddModelError("UserAlias", "Ya existe un usuario con ese alias.");
                        return View(user);
                    }
                    else
                    {
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
                        var userPassGenerate = GenerateRandomPassword();
                        var userPass = new UserPassword();
                        userPass.UserId = query.UserId;
                        userPass.Password = userPassGenerate;
                        _context.Add(userPass);

                        //Guardar Cambios
                        await _context.SaveChangesAsync();
                    }

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
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserIdentification,UserName,UserGender,UserEmail,UserAlias")] User user, int Module)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Traerse Id del usuario creado para Modificar Perfil
                    var query = (from a in _context.Users
                                 where a.UserId == user.UserId
                                 select a).FirstOrDefault();

                    //Modificar Perfil
                    var userProfile = new Profile();
                    userProfile.UserId = query.UserId;
                    userProfile.ModuleId = Module;
                    _context.Update(userProfile);

                    // Actualizar datos del usuario editados
                    query.UserIdentification = user.UserIdentification;
                    query.UserName = user.UserName;
                    query.UserGender = user.UserGender;
                    query.UserEmail = user.UserEmail;
                    query.UserAlias = user.UserAlias;
                    _context.Update(query);

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

        public string GenerateRandomPassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();
            string password = new string(
                Enumerable.Repeat(validChars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return password;
        }
    }
}
