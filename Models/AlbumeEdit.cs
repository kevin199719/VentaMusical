namespace VentaMusical.Models
{
    public class AlbumeEdit
    {
        public int AlbumeId { get; set; }

        public int AuthorId { get; set; }

        public string AlbumeName { get; set; } = null!;

        public DateTime AlbumeYear { get; set; }

        public bool AlbumeState { get; set; }
    }
}
