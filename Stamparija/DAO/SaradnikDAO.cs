using MySql.Data.MySqlClient;
using Stamparija.DTO;
using System.Data.SqlClient;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace Stamparija.DAO
{
    public class SaradnikDAO
    {
        private readonly string _connectionString;

        public SaradnikDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Saradnik> SearchSaradnik(string sifra)
        {
            var saradnik = new List<Saradnik>();
            string query = @"
            SELECT s.sifra, s.mjesto_postanskiBroj, m.naziv , s.naziv, s.ime, s.prezime, s.jib, s.jmb, s.vrsta
            FROM saradnik s
            INNER join mjesto m on s.mjesto_postanskiBroj=m.postanskiBroj
            WHERE s.sifra = @sifra
            ORDER BY sifra ";

            using (var conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@sifra", sifra);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            saradnik.Add( new Saradnik //todo nulls
                            {
                                Sifra = reader.GetString(0),
                                Mjesto = new Mjesto(reader.GetString(1), reader.GetString(2)),
                                Naziv = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                Ime = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                Prezime = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                JIB = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                JMB = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                Vrsta = reader.GetString(8)
                            });
                        }
                    }
                }
            }

            return saradnik;
        }
        public List<Saradnik> GetSaradnici()
        {
            var saradnici = new List<Saradnik>();
            string query = @"
            SELECT s.sifra, s.mjesto_postanskiBroj, m.naziv , s.naziv, s.ime, s.prezime, s.jib, s.jmb, s.vrsta
            FROM saradnik s
            INNER join mjesto m on s.mjesto_postanskiBroj=m.postanskiBroj
            ORDER BY sifra ";

            using (var conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            saradnici.Add(new Saradnik //todo nulls
                            {
                                Sifra = reader.GetString(0),
                                Mjesto = new Mjesto( reader.GetString(1), reader.GetString(2)),
                                Naziv = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                Ime = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                Prezime = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                JIB = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                JMB = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                Vrsta = reader.GetString(8)
                            });
                        }
                    }
                }
            }

            return saradnici;
        }

        public bool AddSaradnik(Saradnik saradnik)
        {
            const string query = "INSERT INTO saradnik " +
                "(sifra, mjesto_postanskiBroj, naziv, ime, prezime, jib, jmb, vrsta) " +
                "VALUES (@sifra, @mjesto_postanskiBroj, @naziv, @ime, @prezime, @jib, @jmb, @vrsta)";

            using (var connection = new MySqlConnection(_connectionString)) {
                using (var command = new MySqlCommand(query, connection)) {
                    command.Parameters.AddWithValue("@sifra", saradnik.Sifra);
                    command.Parameters.AddWithValue("@mjesto_postanskiBroj", saradnik.Mjesto.PostanskiBroj);
                    command.Parameters.AddWithValue("@naziv", saradnik.Naziv);
                    command.Parameters.AddWithValue("@ime", saradnik.Ime);
                    command.Parameters.AddWithValue("@prezime", saradnik.Prezime);
                    command.Parameters.AddWithValue("@jib", saradnik.JIB);
                    command.Parameters.AddWithValue("@jmb", saradnik.JMB);
                    command.Parameters.AddWithValue("@vrsta", saradnik.Vrsta);

                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
            }
        }

        public bool UpdateSaradnik(Saradnik saradnik)
        {
            string query = @"
            UPDATE saradnik
            SET ime = @ime, mjesto_postanskiBroj = @mjesto, naziv = @naziv, prezime = @prezime, 
                jib = @jib, jmb = @jmb, vrsta = @vrsta
            WHERE sifra = @sifra";

            using (var conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@sifra", saradnik.Sifra);
                    cmd.Parameters.AddWithValue("@ime", saradnik.Ime);
                    cmd.Parameters.AddWithValue("@mjesto", saradnik.Mjesto.PostanskiBroj);
                    cmd.Parameters.AddWithValue("@naziv", saradnik.Naziv);
                    cmd.Parameters.AddWithValue("@prezime", saradnik.Prezime);
                    cmd.Parameters.AddWithValue("@jib", (object)saradnik.JIB ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@jmb", (object)saradnik.JMB ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@vrsta", saradnik.Vrsta);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteSaradnik(string sifra)
        {
            //obrisati i telefon todo
            string query = "DELETE FROM saradnik WHERE sifra = @sifra";

            using (var conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@sifra", sifra);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
