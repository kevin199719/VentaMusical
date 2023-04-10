using Microsoft.AspNetCore.Mvc;
//Agrego referencias para las cookies y seguridad de acceso
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using VentaMusical.Models;
using Microsoft.EntityFrameworkCore;
using VentaMusical.Models.Entities;
using VentaMusical.Data;
//**********

namespace VentaMusical.Controllers
{
    public class AccessController : Controller
    {
        public const string SessionKeyId = "_Id";

        private readonly VentaMusicalContext _context;
        public AccessController(VentaMusicalContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();

        }

        public IActionResult AccessDenied()
        {
            return View();

        }

        //Las cookies son asincronicas entonces el metodo es #async Task<"">#
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginView _usuario)
        {
            var user = ValidarUsuario(_usuario.User.UserAlias, _usuario.Password);
            if (user != null)
            {


                //Me traigo los roles del usuario
                var query = new List<Profile>();
                query = _context.Profiles.Where(s => s.UserId == user.UserId).ToList();
                var rolitos = new List<string>();
                foreach (Profile Uri in query)
                {
                    string rolcillo = Uri.ModuleId.ToString();
                    rolitos.Add(rolcillo);
                }

                //Establezco los datos principales en cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("UsuarioAlias", user.UserAlias),
                };

                //Establecemos el role del usuario
                //Leemos con un for los roles que tiene el usuario y los almacena en la cookie
                foreach (string rol in rolitos)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol.ToString()));
                }
                //Guardamos todos los roles
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                //Como es un metodo asincrono debemos ponerlo a esperar
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                //Establezco variable session con Id del usuario
                HttpContext.Session.SetInt32(SessionKeyId, user.UserId);

                //Direccionamos a la pantalla según el rol:
                bool isADMIN = false;
                foreach (string rol in rolitos)
                {
                    if (rol == "1")
                    {
                        isADMIN = true;
                        break;
                    }
                }
                if (isADMIN)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Sales");
                }


            }
            return View();

        }

        public async Task<IActionResult> LogOut()
        {
            //eliminamos las cookies
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("~/Views/Access/Login.cshtml");

        }



        public List<User> GetUsers()
        {
            var users = _context.Users.Include(p => p.Profiles).Include(p => p.UserPasswords).ToList();
            return users;


        }

        public User ValidarUsuario(string alias, string clave)
        {
            string contrasena;
            var user = GetUsers().Where(item => item.UserAlias == alias).FirstOrDefault();
            if (user != null)
            {
                contrasena = user.UserPasswords.FirstOrDefault().Password;
                if (contrasena == clave)
                {
                    return user;
                }
            }
            return null;
        }




    }
}
