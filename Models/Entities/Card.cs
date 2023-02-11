using System;
using System.Collections.Generic;

namespace VentaMusical.Models.Entities;

public partial class Card
{
    public int CardId { get; set; }

    public int CardType { get; set; }

    public string CardNumber { get; set; } = null!;

    public int CardCvv { get; set; }

    public DateTime CardExpiration { get; set; }

    public bool? CardState { get; set; }

    public virtual CardType CardTypeNavigation { get; set; } = null!;

    public virtual ICollection<UsersCard> UsersCards { get; } = new List<UsersCard>();
}
