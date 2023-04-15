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
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

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
			var userCards = await _context.UsersCards.Where(x => x.UsersCardsId == u.Card.CardId).FirstOrDefaultAsync();

			

			if (card == null)
            {
                card = new Card();
                userCards = new UsersCard();

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
				card.CardCvv = u.Card.CardCvv;
				card.CardExpiration = u.Card.CardExpiration;
				card.CardState = true;

				_context.Cards.Add(card);
				await _context.SaveChangesAsync();

				userCards.CardId = card.CardId;
                userCards.UserId = u.User.UserId;
                userCards.UsersCardsState = true;
                
                
                _context.UsersCards.Add(userCards);
			}
            else
            {
				card.CardNumber = u.Card.CardNumber;
			}
         

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

			card.CardCvv = u.Card.CardCvv;
            card.CardExpiration = u.Card.CardExpiration;
            card.CardState = true;

			var password = await _context.UserPasswords.Where(x => x.PasswordId == u.Password.PasswordId).FirstOrDefaultAsync();
            password.Password = u.Password.Password;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



    }


}