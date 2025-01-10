using MySql.Data.MySqlClient;
using Stamparija.DTO;

namespace Stamparija.DAO
{

    public class ArtikalDAO
    {
        private readonly string _connectionString;

        public ArtikalDAO(string connectionString)
        {
            _connectionString = connectionString;
        }
        //testirano
        public List<Artikal> GetArtikli()
        {
            var artikli = new List<Artikal>();
            string query = @"
            SELECT a.sifra, a.naziv, a.kategorija, a.kolicina, a.cijenaBezMarze, a.marza, 
            a.proizvodjac_sifra, p.ime, p.DrzavaPorijekla 
            FROM artikal a 
            INNER JOIN proizvodjac p on a.Proizvodjac_Sifra=p.sifra 
            ORDER BY sifra ASC ";

            using (var conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            artikli.Add(new Artikal
                            {
                                Sifra = reader.GetString(0),
                                Naziv = reader.GetString(1),
                                Kategorija = reader.GetString(2),
                                Kolicina = reader.GetDouble(3),
                                CijenaBezMarze = reader.GetDouble(4),
                                Marza = reader.GetDouble(5),
                                Proizvodjac = new Proizvodjac(
                                    reader.GetString(6), reader.GetString(7), reader.GetString(8))
                            });
                        }
                    }
                }
            }

            return artikli;
        }
        //testirano
        public List<Artikal> SearchArtikli(string kategorija) //treba po kategoriji
        {
            var artikli = new List<Artikal>();
            string query = @"
            SELECT a.sifra, a.naziv, a.kategorija, a.kolicina, a.cijenaBezMarze, a.marza, 
            a.proizvodjac_sifra, p.ime, p.DrzavaPorijekla 
            FROM artikal a
            INNER JOIN proizvodjac p on a.Proizvodjac_Sifra=p.sifra 
            WHERE a.kategorija LIKE @kategorija 
            ORDER BY sifra ASC";

            using (var conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@kategorija", $"%{kategorija}%");
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            artikli.Add(new Artikal
                            {
                                Sifra = reader.GetString(0),
                                Naziv = reader.GetString(1),
                                Kategorija = reader.GetString(2),
                                Kolicina = reader.GetDouble(3),
                                CijenaBezMarze = reader.GetDouble(4),
                                Marza = reader.GetDouble(5),
                                Proizvodjac = new Proizvodjac(
                                    reader.GetString(6), reader.GetString(7), reader.GetString(8))
                            });
                        }
                    }
                }
            }

            return artikli;
        }

        public bool AddArtikal(Artikal artikal)
        {
            string query = @"
            INSERT INTO artikal 
            (sifra, naziv, kolicina, cijenaBezMarze, kategorija, marza, 
             proizvodjac_sifra) 
            VALUES (@sifra, @naziv, @kolicina, @cijenaBezMarze, @kategorija, @marza, 
                     @proizvodjacSifra)";

            using (var conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@sifra", artikal.Sifra);
                    cmd.Parameters.AddWithValue("@naziv", artikal.Naziv);
                    cmd.Parameters.AddWithValue("@kolicina", artikal.Kolicina);
                    cmd.Parameters.AddWithValue("@cijenaBezMarze", artikal.CijenaBezMarze);
                    cmd.Parameters.AddWithValue("@kategorija", artikal.Kategorija);
                    cmd.Parameters.AddWithValue("@marza", artikal.Marza);
                    cmd.Parameters.AddWithValue("@proizvodjacSifra", artikal.Proizvodjac);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateArtikal(Artikal artikal)
        {
            string query = @"
            UPDATE artikal 
            SET naziv = @naziv, kolicina = @kolicina, cijenaBezMarze = @cijenaBezMarze, 
                kategorija = @kategorija, marza = @marza, proizvodjac_sifra = @proizvodjacSifra 
            WHERE sifra = @sifra";

            using (var conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@naziv", artikal.Naziv);
                    cmd.Parameters.AddWithValue("@kolicina", artikal.Kolicina);
                    cmd.Parameters.AddWithValue("@cijenaBezMarze", artikal.CijenaBezMarze);
                    cmd.Parameters.AddWithValue("@kategorija", artikal.Kategorija);
                    cmd.Parameters.AddWithValue("@marza", artikal.Marza);
                    cmd.Parameters.AddWithValue("@proizvodjacSifra", artikal.Proizvodjac);
                    cmd.Parameters.AddWithValue("@sifra", artikal.Sifra);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteArtikal(string sifra)
        {
            string query = "DELETE FROM artikal WHERE sifra = @sifra";

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
