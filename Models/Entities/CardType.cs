using System;
using System.Collections.Generic;

namespace VentaMusical.Models.Entities;

public partial class CardType
{
    public int CardTypeId { get; set; }

    public string CardName { get; set; } = null!;

    public virtual ICollection<Card> Cards { get; } = new List<Card>();
}
