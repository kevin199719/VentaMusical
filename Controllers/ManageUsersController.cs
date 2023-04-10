using Microsoft.AspNetCore.Mvc;
using VentaMusical.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VentaMusical.ViewModels;
using VentaMusical.Models.Entities;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace VentaMusical.Controllers
{
	public class ManageUsersController : Controller
	{
		private readonly VentaMusicalContext _context;
		public const string SessionKeyId = "_Id";

		public ManageUsersController(VentaMusicalContext context)
		{
			_context = context;
		}

		//GET:Users

		public async Task<IActionResult> Index()
		{
			int? userId = HttpContext.Session.GetInt32(SessionKeyId);
			var user = await _context.Users.Include(p => p.UserPasswords).Include(uc => uc.UsersCards).ThenInclude(c => c.Card).FirstOrDefaultAsync(x => x.UserId == userId);
			var viewModel = new UserViewModel();
			viewModel.User = user;
			//Agregar logica de defaults
			viewModel.Card = user.UsersCards.FirstOrDefault()?.Card;
			viewModel.Password = user.UserPasswords.FirstOrDefault();

			return View(viewModel);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(UserViewModel u)
		{
			int? userId = HttpContext.Session.GetInt32(SessionKeyId);
			var user = await _context.Users.Include(p => p.UserPasswords).Include(uc => uc.UsersCards).ThenInclude(c => c.Card).FirstOrDefaultAsync(x => x.UserId == userId);
			user.UserName = u.User.UserName;
			user.UserEmail = u.User.UserEmail;
			user.UserGender = u.User.UserGender;
			var card = await _context.Cards.Where(x => x.CardId == u.Card.CardId).FirstOrDefaultAsync();
						card.CardNumber = u.Card.CardNumber;

			if (u.Card.CardNumber.StartsWith("3"))
			{
				card.CardType = 1;

			}
			else if (u.Card.CardNumber.StartsWith("4"))
			{
				card.CardType = 2;
			}
			else if (u.Card.CardNumber.StartsWith("5"))
			{
				card.CardType = 3;
			}
			else
			{
				card.CardType = 4;
			}

			var password = await _context.UserPasswords.Where(x => x.PasswordId == u.Password.PasswordId).FirstOrDefaultAsync();
			password.Password = u.Password.Password;
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}

	}


}