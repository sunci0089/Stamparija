using MySql.Data.MySqlClient;
using Stamparija.DTO;

namespace Stamparija.DAO
{
    public class ProizvodjacDAO
    {
        private readonly string _connectionString;

        public ProizvodjacDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Proizvodjac> SearchProizvodjac(string sifra)
        {
            var retVal = new List<Proizvodjac>();

            const string query = @"SELECT sifra, ime, drzavaPorijekla
                               FROM proizvodjac
                               WHERE sifra = @sifra";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@sifra", sifra);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                retVal.Add(new Proizvodjac(
                    reader.GetString("sifra"),
                    reader.GetString("ime"),
                    reader.GetString("drzavaPorijekla")
                ));
            }

            return retVal;
        }

        public List<Proizvodjac> GetProizvodjaci()
        {
            var retVal = new List<Proizvodjac>();

            const string query = @"SELECT sifra, ime, drzavaPorijekla
                               FROM proizvodjac
                               ORDER BY sifra ASC";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                retVal.Add(new Proizvodjac(
                    reader.GetString("sifra"),
                    reader.GetString("ime"),
                    reader.GetString("drzavaPorijekla")
                ));
            }

            return retVal;
        }

        public bool AddProizvodjac(Proizvodjac proizvodjac)
        {
            const string query = @"INSERT INTO proizvodjac (sifra, ime, drzavaPorijekla) 
                               VALUES (@sifra, @ime, @drzavaPorijekla)";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@sifra", proizvodjac.Sifra);
            command.Parameters.AddWithValue("@ime", proizvodjac.Ime);
            command.Parameters.AddWithValue("@drzavaPorijekla", proizvodjac.DrzavaPorijekla);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }

        public bool UpdateProizvodjac(Proizvodjac proizvodjac)
        {
            const string query = @"UPDATE proizvodjac
                               SET ime = @ime, drzavaPorijekla = @drzavaPorijekla
                               WHERE sifra = @sifra";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ime", proizvodjac.Ime);
            command.Parameters.AddWithValue("@drzavaPorijekla", proizvodjac.DrzavaPorijekla);
            command.Parameters.AddWithValue("@sifra", proizvodjac.Sifra);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }

        public bool DeleteProizvodjac(string sifra)
        {
            const string query = "DELETE FROM proizvodjac WHERE sifra = @sifra";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@sifra", sifra);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }
    }

}
