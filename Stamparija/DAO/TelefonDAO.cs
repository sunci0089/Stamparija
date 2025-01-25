using MySql.Data.MySqlClient;
using Stamparija.DAO.connection;
using Stamparija.DTO;

namespace Stamparija.DAO
{
    public class TelefonDAO
    {
        private readonly string _connectionString;

        public TelefonDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Telefon> GetTelefoni()
        {
            var retVal = new List<Telefon>();

            const string query = @"SELECT saradnik_sifra, brTel
                               FROM telefon ";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                retVal.Add(new Telefon(
                    reader.GetString("brTel"),
                    MySQLDataAccessFactory.GetSaradnikDataAccess().SearchSaradnik(reader.GetString("saradnik_sifra"))[0]
                ));
            }

            return retVal;
        }

        public List<Telefon> SearchTelefon(string brtel)
        {
            var retVal = new List<Telefon>();

            const string query = @"SELECT brTel, saradnik_sifra 
                               FROM telefon
                               WHERE brTel LIKE @brojTelefona
                               ORDER BY saradnik_sifra ASC";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@brojTelefona", brtel);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                retVal.Add(new Telefon(
                    reader.GetString("brTel"),
                    MySQLDataAccessFactory.GetSaradnikDataAccess().SearchSaradnik(reader.GetString("saradnik_sifra"))[0]

                ));
            }

            return retVal;
        }

        public bool AddTelefon(Telefon telefon)
        {
            const string query = "INSERT INTO telefon (saradnik_sifra, brTel) VALUES (@saradnik_sifra, @brTel)";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@saradnik_sifra", telefon.Saradnik);
            command.Parameters.AddWithValue("@brTel", telefon.BrTel);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }

        public bool UpdateTelefon(Telefon telefon, string oldBrTel)
        {
            const string query = @"UPDATE telefon 
                               SET brTel = @brTel
                               WHERE saradnik_sifra = @saradnik_sifra AND brTel = @oldBrTel";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@brTel", telefon.BrTel);
            command.Parameters.AddWithValue("@saradnik_sifra", telefon.Saradnik);
            command.Parameters.AddWithValue("@oldBrTel", oldBrTel);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }

        public bool DeleteTelefon(string saradnikSifra, string brTel)
        {
            const string query = "DELETE FROM telefon WHERE saradnik_sifra = @saradnik_sifra AND brTel = @brTel";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@saradnik_sifra", saradnikSifra);
            command.Parameters.AddWithValue("@brTel", brTel);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }
    }

}
