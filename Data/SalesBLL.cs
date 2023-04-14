using Microsoft.Data.SqlClient;
using System.Data;
using VentaMusical.Models;
using VentaMusical.Models.Entities;

namespace VentaMusical.Data
{
    public class SalesBLL
    {
        public static string connectionString = "Server=DESKTOP-8LPUDSS;Database=VentaMusical;Trusted_Connection=True;TrustServerCertificate=True";

        public static List<SongModel> getSongs(string SongName, string AlbumeName, string AuthorName, string GenderDescription)
        {
            List<SongModel> songs = new List<SongModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT s.SongId, s.SongName, a.AuthorName +' - '+al.AlbumeName as Descript  FROM Song s join Author a on s.AuthorId = a.AuthorId join Albume al on al.AlbumeId = s.AlbumeId join Gender g on s.GenderId = g.GenderId where s.SongState=1 and s.SongName like'%" + SongName + "%' and al.AlbumeName like'%" + AlbumeName + "%' and a.AuthorName like'%" + AuthorName + "%' and g.GenderDescription like'%" + GenderDescription + "%'", connection);

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


        public static int AddSongToInvoice(int userId, int songId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("AddSongToInvoice", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@songId", songId);

                    command.ExecuteNonQuery();
                }
            }
            return 1;
        }

        public static List<SongModel> getInvoiceSongs(int userId)
        {
            List<SongModel> songs = new List<SongModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT id.InvoiceDetailId,s.SongId, s.SongName,'Del artista y albume: '+ a.AuthorName +' - '+al.AlbumeName as Descript,s.SongPrice * 0.87 as 'SongPrice',s.SongPrice * 0.13 as 'IVA' FROM InvoiceHeader i join InvoiceDetail id on i.InvoiceHeaderId = id.InvoiceHeaderId join Song s on id.SongId = s.SongId join Author a on s.AuthorId = a.AuthorId join Albume al on al.AlbumeId = s.AlbumeId join Gender g on s.GenderId = g.GenderId where i.InvoiceState=1 AND i.UserId = " + userId, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SongModel song = new SongModel();
                        song.InvoiceDetailId = reader.GetInt32(0);
                        song.SongId = reader.GetInt32(1);
                        song.SongName = reader.GetString(2);
                        song.Description = reader.GetString(3);
                        song.SongPrice = reader.GetDecimal(4);
                        song.IVA = reader.GetDecimal(5);
                        songs.Add(song);
                    }
                }
            }

            return songs;
        }

        public static UserCard getUserCard(int userId)
        {
            UserCard card = new UserCard();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select c.CardId,c.CardNumber,c.CardExpiration,ct.CardName,u.UserName,u.UserEmail from Users_Cards uc join [Card] c on uc.CardId = c.CardId join CardType ct on c.CardType = ct.CardTypeId join Users u on u.UserId = uc.UserId where uc.UserId=" + userId + "and Users_Cards_State=1", connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        card.CardId = reader.GetInt32(0);
                        card.CardNumber = reader.GetString(1);
                        card.CardExpiration = reader.GetDateTime(2).ToString();
                        card.CardName = reader.GetString(3);
                        card.UserName = reader.GetString(4);
                        card.UserEmail = reader.GetString(5);
                    }
                }
            }

            return card;
        }

        public static void CleanItem(int invoiceDetailId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("delete from InvoiceDetail where InvoiceDetailId=@id", connection);
                command.Parameters.AddWithValue("@id", invoiceDetailId);
                command.ExecuteNonQuery();
            }
        }

        public static int PayInvoice(int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("PayInvoice", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@userId", userId);

                    command.ExecuteNonQuery();
                }
            }
            return 1;
        }

    }
}
