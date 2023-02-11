using System;
using System.Collections.Generic;

namespace VentaMusical.Models.Entities;

public partial class Author
{
    public int AuthorId { get; set; }

    public string AuthorName { get; set; } = null!;

    public bool AuthorState { get; set; }

    public virtual ICollection<Albume> Albumes { get; } = new List<Albume>();

    public virtual ICollection<Song> Songs { get; } = new List<Song>();
}
