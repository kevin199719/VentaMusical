using System;
using System.Collections.Generic;

namespace VentaMusical.Models.Entities;

public partial class Module
{
    public int ModuleId { get; set; }

    public string ModuleName { get; set; } = null!;

    public virtual ICollection<Profile> Profiles { get; } = new List<Profile>();
}
