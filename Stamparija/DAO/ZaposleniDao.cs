using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
using Stamparija.DTO;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Stamparija.DAO
{
    public class ZaposleniDAO
    {
        private readonly string _connectionString;

        public ZaposleniDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Zaposleni> SearchZaposleni(string id) //ne dohvacati password to je samo hash
        {
            var retVal = new List<Zaposleni>();

            const string query = @"SELECT id, ime, prezime, jmb, username, password, isAdmin
                               FROM zaposleni
                               WHERE id LIKE @id";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                retVal.Add(new Zaposleni(
                    reader.GetInt32("id"),
                    reader.GetString("ime"),
                    reader.GetString("prezime"),
                    reader.GetString("jmb"),
                    reader.GetString("username"),
                    reader.GetString("password"),
                    reader.GetByte("isAdmin")
                ));
            }

            return retVal;
        }

        public Zaposleni login(string username, string hash)
        {
            Zaposleni retVal = null;
            const string query = @"SELECT id, ime, prezime, jmb, username, password, isAdmin
                               FROM zaposleni
                               WHERE username = @username and password = @hash";
            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@hash", hash);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                retVal = new Zaposleni(
                    reader.GetInt32("id"),
                    reader.GetString("ime"),
                    reader.GetString("prezime"),
                    reader.GetString("jmb"),
                    reader.GetString("username"),
                    reader.GetString("password"),
                    reader.GetByte("isAdmin")
                );
            }
            return retVal;
        }
        public List<Zaposleni> GetZaposleni()
        {
            var retVal = new List<Zaposleni>();

            const string query = @"SELECT id, ime, prezime, jmb, username, password, isAdmin
                               FROM zaposleni
                               ORDER BY id ASC";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                retVal.Add(new Zaposleni(
                    reader.GetInt32("id"),
                    reader.GetString("ime"),
                    reader.GetString("prezime"),
                    reader.GetString("jmb"),
                    reader.GetString("username"),
                    reader.GetString("password"),
                    reader.GetByte("isAdmin")
                ));
            }

            return retVal;
        }

        public bool AddZaposleni(Zaposleni zaposleni) //todo izbaciti unos id-a //polje neizmjenjivo
        {
            const string query = @"INSERT INTO zaposleni (id, ime, prezime, jmb, username, password, isAdmin) 
                               VALUES (@id, @ime, @prezime, @jmb, @username, @password, @isAdmin)";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", zaposleni.id);
            command.Parameters.AddWithValue("@ime", zaposleni.ime);
            command.Parameters.AddWithValue("@prezime", zaposleni.prezime);
            command.Parameters.AddWithValue("@jmb", zaposleni.jmb);
            command.Parameters.AddWithValue("@username", zaposleni.username);
            command.Parameters.AddWithValue("@password", zaposleni.password);
            command.Parameters.AddWithValue("@isAdmin", zaposleni.isAdmin);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }

        public bool UpdateZaposleni(Zaposleni zaposleni)
        {
            const string query = @"UPDATE zaposleni
                               SET ime = @ime, prezime = @prezime, jmb = @jmb, username = @username, password = @password, isAdmin = @isAdmin
                               WHERE id = @id";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ime", zaposleni.ime);
            command.Parameters.AddWithValue("@prezime", zaposleni.prezime);
            command.Parameters.AddWithValue("@jmb", zaposleni.jmb);
            command.Parameters.AddWithValue("@username", zaposleni.username);
            command.Parameters.AddWithValue("@password", zaposleni.password);
            command.Parameters.AddWithValue("@isAdmin", zaposleni.isAdmin);
            command.Parameters.AddWithValue("@id", zaposleni.id);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }

        public bool DeleteZaposleni(int id)
        {
            const string query = "DELETE FROM zaposleni WHERE id = @id";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }
    }
}
