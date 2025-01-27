using MySql.Data.MySqlClient;
using Stamparija.DAO.connection;
using Stamparija.DTO;

namespace Stamparija.DAO
{
    public class OtkupStavkaDAO
    {
        private readonly string _connectionString;

        public OtkupStavkaDAO(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<OtkupStavka> GetOtkupStavka()
        {
            List<OtkupStavka> stavke = new List<OtkupStavka>();
            const string query = @"SELECT otkup_brojPotvrde, roba_sifra, kolicina 
            FROM otkup_stavka 
            ORDER BY otkup_brojPotvrde ";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var otkupList = MySQLDataAccessFactory.GetOtkupDataAccess().SearchOtkup(reader.GetString(0));
                var artikalList = MySQLDataAccessFactory.GetArtikalDataAccess().SearchArtikliBySifra(reader.GetString(1));

                if (otkupList != null && otkupList.Count > 0 && artikalList != null && artikalList.Count > 0)
                {
                    stavke.Add(new OtkupStavka(
                        otkupList[0],
                        artikalList[0],
                        reader.GetDouble(2)
                    ));
                }
            }

            return stavke;
        }
        public List<OtkupStavka> SearchOtkupStavka(string sifraOtkupa)
        {
            List<OtkupStavka> stavke = new List<OtkupStavka>();
            const string query = @"SELECT os.otkup_brojPotvrde, os.roba_sifra, os.kolicina 
            FROM otkup_stavka os
            WHERE os.otkup_brojPotvrde = @sifraOtkupa 
            ORDER BY roba_sifra ";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@sifraOtkupa", sifraOtkupa);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var otkupList = MySQLDataAccessFactory.GetOtkupDataAccess().SearchOtkup(reader.GetString(0));
                var artikalList = MySQLDataAccessFactory.GetArtikalDataAccess().SearchArtikliBySifra(reader.GetString(1));

                if (otkupList != null && otkupList.Count > 0 && artikalList != null && artikalList.Count > 0)
                {
                    stavke.Add(new OtkupStavka(
                        otkupList[0],
                        artikalList[0],
                        reader.GetDouble(2)
                    ));
                }
            }

            return stavke;
        }

        public bool AddOStavka(OtkupStavka stavka)
        {
            const string query = "INSERT INTO otkup_stavka (otkup_brojPotvrde, kolicina, roba_sifra) VALUES (@brojPotvrde, @kolicina, @sifraRobe)";
            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@brojPotvrde", stavka.otkup.sifra);
            command.Parameters.AddWithValue("@kolicina", stavka.kolicina);
            command.Parameters.AddWithValue("@sifraRobe", stavka.artikal.Sifra);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }

        public bool UpdateOStavka(OtkupStavka stavka)
        {
            const string query = @"UPDATE otkup_stavka SET 
                                kolicina = @kolicina
                                WHERE otkup_brojPotvrde = @brojPotvrde AND roba_sifra = @sifraRobe";
            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@kolicina", stavka.kolicina);
            command.Parameters.AddWithValue("@brojPotvrde", stavka.otkup.sifra);
            command.Parameters.AddWithValue("@sifraRobe", stavka.artikal.Sifra);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }

        public bool DeleteOStavka(string sifraOtkupa, string sifraRobe)
        {
            const string query = "DELETE FROM otkup_stavka WHERE otkup_brojPotvrde = @sifraOtkupa AND roba_sifra = @sifraRobe";
            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@sifraOtkupa", sifraOtkupa);
            command.Parameters.AddWithValue("@sifraRobe", sifraRobe);

            connection.Open();
            return command.ExecuteNonQuery() == 1;
        }
    }

}
