using NuGet.Protocol.Core.Types;
using VentaMusical.Models.Entities;

namespace VentaMusical.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }
        public Card Card { get; set; }
        public CardType CardType { get; set; }
        public UserPassword Password { get; set; }

      

    }
}
