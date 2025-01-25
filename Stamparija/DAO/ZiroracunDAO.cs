using MySql.Data.MySqlClient;
using Stamparija.DAO.connection;
using Stamparija.DTO;


namespace Stamparija.DAO
{
    public class ZiroracunDAO
    {
        private readonly string _connectionString;

        public ZiroracunDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Ziroracun> GetZiroracun()
        {
            var retVal = new List<Ziroracun>();

            const string query = @"SELECT z.brojRacuna, z.banka, z.saradnici_sifra
                               FROM ziroRacun z
                               ORDER BY z.brojRacuna ASC";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                retVal.Add(new Ziroracun(
                    reader.GetString("brojRacuna"),
                    MySQLDataAccessFactory.GetSaradnikDataAccess().SearchSaradnik(reader.GetString("saradnici_sifra"))[0],
                    reader.GetString("banka")
                ));
            }

            return retVal;
        }

        public List<Ziroracun> SearchZiroracun(string brojRacuna)
        {
            var retVal = new List<Ziroracun>();

            const string query = @"SELECT z.brojRacuna, z.banka, z.saradnici_sifra
                               FROM ziroRacun z
                               WHERE z.brojRacuna LIKE @brojRacuna ";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@brojRacuna", brojRacuna);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                retVal.Add(new Ziroracun(
                    reader.GetString("brojRacuna"),
                    MySQLDataAccessFactory.GetSaradnikDataAccess().SearchSaradnik(reader.GetString("saradnici_sifra"))[0],
                    reader.GetString("banka")
                ));
            }

            return retVal;
        }

        public bool AddZiroracun(Ziroracun ziroRacun)
        {
            const string query = "INSERT INTO ziroRacun (brojRacuna, banka, saradnici_sifra) VALUES (@brojRacuna, @banka, @saradnici_sifra)";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@brojRacuna", ziroRacun.brojRacuna);
            command.Parameters.AddWithValue("@banka", ziroRacun.banka);
            command.Parameters.AddWithValue("@saradnici_sifra", ziroRacun.saradnik);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }

        public bool UpdateZiroracun(Ziroracun ziroRacun)
        {
            const string query = @"UPDATE ziroRacun 
                               SET banka = @banka, saradnici_sifra = @saradnici_sifra
                               WHERE brojRacuna = @brojRacuna";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@banka", ziroRacun.banka);
            command.Parameters.AddWithValue("@saradnici_sifra", ziroRacun.saradnik);
            command.Parameters.AddWithValue("@brojRacuna", ziroRacun.brojRacuna);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }

        public bool DeleteZiroracun(string brojRacuna)
        {
            const string query = "DELETE FROM ziroRacun WHERE brojRacuna = @brojRacuna";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@brojRacuna", brojRacuna);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }
    }

}
