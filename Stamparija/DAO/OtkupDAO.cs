using MySql.Data.MySqlClient;
using Stamparija.DAO.connection;
using Stamparija.DTO;
using System.Data;

namespace Stamparija.DAO
{

    public class OtkupDAO
    {
        private readonly string _connectionString;

        public OtkupDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

       public List<Otkup> SearchOtkup(string brojPotvrde)
        {
            var retVal = new List<Otkup>();

            const string query = @"SELECT o.brojPotvrde, o.faktura_sifra, o.saradnici_sifra 
                               FROM otkup o
                               WHERE o.brojPotvrde LIKE @brojPotvrde 
                               ORDER BY o.brojPotvrde ASC";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@brojPotvrde", brojPotvrde);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                retVal.Add(new Otkup(
                    reader.GetString("brojPotvrde"),
                    MySQLDataAccessFactory.GetFakturaDataAccess().SearchFakture(reader.GetString("faktura_sifra"))[0],
                    MySQLDataAccessFactory.GetSaradnikDataAccess().SearchSaradnik(reader.GetString("saradnici_sifra"))[0]

                    ));
            }

            return retVal;
        }

       public List<Otkup> GetOtkupi()
        {
            var retVal = new List<Otkup>();

            const string query = @"SELECT o.brojPotvrde, o.faktura_sifra, o.saradnici_sifra 
                               FROM otkup o
                               ORDER BY o.brojPotvrde ASC";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                retVal.Add(new Otkup(
                    reader.GetString("brojPotvrde"),
                    MySQLDataAccessFactory.GetFakturaDataAccess().SearchFakture(reader.GetString("faktura_sifra"))[0],
                    MySQLDataAccessFactory.GetSaradnikDataAccess().SearchSaradnik(reader.GetString("saradnici_sifra"))[0]

                    ));
            }

            return retVal;
        }

        public bool AddOtkup(Otkup otkup)
        {
            const string query = "INSERT INTO otkup (brojPotvrde, faktura_sifra, saradnici_sifra) " +
                "VALUES (@brojPotvrde, @faktura_sifra, @saradnici_sifra)";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@brojPotvrde", otkup.sifra);
            command.Parameters.AddWithValue("@faktura_sifra", otkup.faktura.Sifra);
            command.Parameters.AddWithValue("@saradnici_sifra", otkup.saradnik.Sifra);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }

        public bool UpdateOtkup(Otkup otkup)
        {
            const string query = @"UPDATE otkup 
                               SET faktura_sifra = @faktura_sifra, 
                                   saradnici_sifra = @saradnici_sifra 
                               WHERE brojPotvrde = @brojPotvrde";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@faktura_sifra", otkup.faktura.Sifra);
            command.Parameters.AddWithValue("@saradnici_sifra", otkup.saradnik.Sifra);
            command.Parameters.AddWithValue("@brojPotvrde", otkup.sifra);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }

        public bool DeleteOtkup(string brojPotvrde)
        {
            const string query = "DELETE FROM otkup WHERE brojPotvrde = @brojPotvrde";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@brojPotvrde", brojPotvrde);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }
    }

}
