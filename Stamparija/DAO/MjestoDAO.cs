using MySql.Data.MySqlClient;
using Stamparija.DTO;


namespace Stamparija.DAO
{

    using System;
    using System.Collections.Generic;
    using MySql.Data.MySqlClient;

    public class MjestoDAO
    {
        private readonly string _connectionString;

        public MjestoDAO(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Mjesto> GetMjesta(string postanskiBrojPattern)
        {
            var mjesta = new List<Mjesto>();
            string query = @"
            SELECT postanskiBroj, naziv
            FROM mjesto
            WHERE postanskiBroj LIKE @postanskiBrojPattern
            ORDER BY postanskiBroj ASC";

            using (var conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@postanskiBrojPattern", $"%{postanskiBrojPattern}%");
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mjesta.Add(new Mjesto
                            {
                                PostanskiBroj = reader.GetString("postanskiBroj"),
                                Naziv = reader.GetString("naziv")
                            });
                        }
                    }
                }
            }

            return mjesta;
        }

        public bool AddMjesto(Mjesto mjesto)
        {
            string query = "INSERT INTO mjesto (postanskiBroj, naziv) VALUES (@postanskiBroj, @naziv)";

            using (var conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@postanskiBroj", mjesto.PostanskiBroj);
                    cmd.Parameters.AddWithValue("@naziv", mjesto.Naziv);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateMjesto(Mjesto mjesto)
        {
            string query = "UPDATE mjesto SET naziv = @naziv WHERE postanskiBroj = @postanskiBroj";

            using (var conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@postanskiBroj", mjesto.PostanskiBroj);
                    cmd.Parameters.AddWithValue("@naziv", mjesto.Naziv);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteMjesto(string postanskiBroj)
        {
            string query = "DELETE FROM mjesto WHERE postanskiBroj = @postanskiBroj";

            using (var conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@postanskiBroj", postanskiBroj);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }


}
