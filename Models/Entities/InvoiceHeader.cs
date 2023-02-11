using System;
using System.Collections.Generic;

namespace VentaMusical.Models.Entities;

public partial class InvoiceHeader
{
    public int InvoiceHeaderId { get; set; }

    public int UserId { get; set; }

    public DateTime InvoiceDate { get; set; }

    public bool InvoiceState { get; set; }

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; } = new List<InvoiceDetail>();
}
