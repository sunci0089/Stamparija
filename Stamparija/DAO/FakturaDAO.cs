using Stamparija.DTO;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace Stamparija.DAO
{
    public class FakturaDAO
    {
        private readonly string _connectionString;

        public FakturaDAO(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Faktura> GetFakture()
        {
            var fakture = new List<Faktura>();
            const string query = @"SELECT 
                                sifra, datumVrijeme, nacinPlacanja, 
                                ziroracun_saradnika, vrstaUplate, 
                                cijenaSaPDV 
                                FROM faktura 
                                ORDER BY sifra ASC";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                fakture.Add(new Faktura(
                    reader.GetString(0),
                    reader.GetDateTime(1),//.ToString("yyyy-MM-dd HH:mm"),
                    reader.GetString(2),
                    reader.IsDBNull(3) ? null : reader.GetString(3),
                    reader.GetString(4),
                    reader.GetDouble(5)));
            }

            return fakture;
        }
        public List<Faktura> SearchFakture(string sifra)
        {
            var fakture = new List<Faktura>();
            const string query = @"SELECT 
                                sifra, datumVrijeme, nacinPlacanja, 
                                ziroracun_saradnika, vrstaUplate, 
                                cijenaSaPDV 
                                FROM faktura 
                                WHERE sifra LIKE @sifra 
                                ORDER BY sifra ASC";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@sifra", $"%{sifra}%");

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                fakture.Add(new Faktura(
                    reader.GetString(0),
                    reader.GetDateTime(1),//.ToString("yyyy-MM-dd HH:mm"),
                    reader.GetString(2),
                    reader.IsDBNull(3) ? null : reader.GetString(3),
                    reader.GetString(4),
                    reader.GetDouble(5)));
            }

            return fakture;
        }

        public bool AddFaktura(Faktura faktura)
        {
            const string query = "INSERT INTO faktura VALUES (@sifra, @datumVrijeme, @nacinPlacanja, @ziroracun, @vrstaUplate, @cijena)";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@sifra", faktura.Sifra);
            command.Parameters.AddWithValue("@datumVrijeme", faktura.DatumVrijeme);//DateTime.ParseExact(faktura.DatumVrijeme+":00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@nacinPlacanja", faktura.NacinPlacanja);
            command.Parameters.AddWithValue("@ziroracun", faktura.ZiroracunSaradnika ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@vrstaUplate", faktura.VrstaUplate);
            command.Parameters.AddWithValue("@cijena", faktura.CijenaSaPDV);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }

        public bool UpdateFaktura(Faktura faktura)
        {
            const string query = @"UPDATE faktura SET 
                                datumVrijeme = @datumVrijeme, 
                                nacinPlacanja = @nacinPlacanja, 
                                ziroracun_saradnika = @ziroracun, 
                                vrstaUplate = @vrstaUplate, 
                                cijenaSaPDV = @cijena 
                                WHERE sifra = @sifra ";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@datumVrijeme", faktura.DatumVrijeme);
            command.Parameters.AddWithValue("@nacinPlacanja", faktura.NacinPlacanja);
            command.Parameters.AddWithValue("@ziroracun", faktura.ZiroracunSaradnika ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@vrstaUplate", faktura.VrstaUplate);
            command.Parameters.AddWithValue("@cijena", faktura.CijenaSaPDV);
            command.Parameters.AddWithValue("@sifra", faktura.Sifra);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }

        public bool DeleteFaktura(string sifra)
        {
            const string query = "DELETE FROM faktura WHERE sifra = @sifra";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@sifra", sifra);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }
    }
}
