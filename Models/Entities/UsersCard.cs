using System;
using System.Collections.Generic;

namespace VentaMusical.Models.Entities;

public partial class UsersCard
{
    public int UsersCardsId { get; set; }

    public int UserId { get; set; }

    public int CardId { get; set; }

    public bool? UsersCardsState { get; set; }

    public virtual Card Card { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
