namespace VentaMusical.Models
{
    public class SongModel
    {
        public int InvoiceDetailId { get; set; }
        public int SongId { get; set; }
        public string SongName { get; set; }
        public string Description { get; set; }
        public decimal? SongPrice { get; set; }
        public decimal? IVA { get; set; }
    }
}
