using System;
using System.Collections.Generic;

namespace VentaMusical.Models.Entities;

public partial class InvoiceDetail
{
    public int InvoiceHeaderId { get; set; }

    public int SongId { get; set; }

    public int InvoiceDetailId { get; set; }

    public virtual InvoiceHeader InvoiceHeader { get; set; } = null!;

    public virtual Song Song { get; set; } = null!;
}
