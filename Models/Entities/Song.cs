using System;
using System.Collections.Generic;

namespace VentaMusical.Models.Entities;

public partial class Song
{
    public int SongId { get; set; }

    public string SongName { get; set; } = null!;

    public int AuthorId { get; set; }

    public int GenderId { get; set; }

    public DateTime SongYear { get; set; }

    public decimal SongPrice { get; set; }

    public bool SongState { get; set; }

    public virtual ICollection<Albume> Albumes { get; } = new List<Albume>();

    public virtual Author Author { get; set; } = null!;

    public virtual Gender Gender { get; set; } = null!;

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; } = new List<InvoiceDetail>();
}
