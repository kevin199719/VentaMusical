using System;
using System.Collections.Generic;

namespace VentaMusical.Models.Entities;

public partial class Gender
{
    public int GenderId { get; set; }

    public string GenderDescription { get; set; } = null!;

    public bool? GenderState { get; set; }

    public virtual ICollection<Song> Songs { get; } = new List<Song>();
}
