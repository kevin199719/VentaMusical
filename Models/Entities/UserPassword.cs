using System;
using System.Collections.Generic;

namespace VentaMusical.Models.Entities;

public partial class UserPassword
{
    public int UserId { get; set; }

    public string Password { get; set; } = null!;

    public int PasswordId { get; set; }

    public virtual User User { get; set; } = null!;
}
