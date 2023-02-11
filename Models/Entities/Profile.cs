using System;
using System.Collections.Generic;

namespace VentaMusical.Models.Entities;

public partial class Profile
{
    public int UserId { get; set; }

    public int ModuleId { get; set; }

    public int ProfileId { get; set; }

    public virtual Module Module { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
