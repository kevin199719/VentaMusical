using Microsoft.Data.SqlClient;
using VentaMusical.Models;

namespace VentaMusical.Data
{
    public class SalesBLL
    {

        public static List<SongModel> getSongs()
        {
            List<SongModel> songs = new List<SongModel>();

            using (SqlConnection connection = new SqlConnection("Server=FUNDA-I7-05\\MSSQLSERVER01;Database=VentaMusical;Trusted_Connection=True;TrustServerCertificate=True"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT s.SongId, s.SongName, a.AuthorName +' - '+al.AlbumeName as Descript  FROM Song s join Author a on s.AuthorId = a.AuthorId join Albume al on al.AlbumeId = s.AlbumeId where s.SongState=1", connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SongModel song = new SongModel();
                        song.SongId = reader.GetInt32(0);
                        song.SongName = reader.GetString(1);
                        song.Description = reader.GetString(2);
                        songs.Add(song);
                    }
                }
            }

            return songs;
        }



    }
}
