using System;
using System.Collections.Generic;

namespace VentaMusical.Models.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string UserIdentification { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public int UserGender { get; set; }

    public string UserEmail { get; set; } = null!;

    public string UserAlias { get; set; } = null!;

    public bool? UserState { get; set; }

    public virtual ICollection<Profile> Profiles { get; } = new List<Profile>();

    public virtual ICollection<UserPassword> UserPasswords { get; } = new List<UserPassword>();

    public virtual ICollection<UsersCard> UsersCards { get; } = new List<UsersCard>();
}
