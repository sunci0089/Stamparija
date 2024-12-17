using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography;
using Stamparija.DTO;

namespace Stamparija.DAO
{
    public class ZaposleniDao
    {
        private Zaposleni zaposleni {  get; set; }
        public static (string Hash, string Salt) HashPassword(string password)
        {
            // Generate a salt
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            // Combine password and salt
            string saltedPassword = password + salt;

            // Hash the salted password
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                string hash = Convert.ToBase64String(hashBytes);
                return (hash, salt);
            }
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            string saltedPassword = password + storedSalt;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                string computedHash = Convert.ToBase64String(hashBytes);
                return computedHash == storedHash;
            }
        }



        public void RegisterUser(string username, string password)
        {
            var (hash, salt) = PasswordHelper.HashPassword(password);

            string connectionString = "YourConnectionStringHere";
            string query = "INSERT INTO Users (Username, PasswordHash, Salt) VALUES (@Username, @PasswordHash, @Salt)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", hash);
                command.Parameters.AddWithValue("@Salt", salt);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public bool LoginUser(string username, string password)
        {
            string connectionString = "YourConnectionStringHere";
            string query = "SELECT PasswordHash, Salt FROM Users WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedHash = reader.GetString(0);
                        string storedSalt = reader.GetString(1);
                        return PasswordHelper.VerifyPassword(password, storedHash, storedSalt);
                    }
                }
            }
            return false;
        }



        /// <summary>
        /// ADMINISTRATORSKE FUNKCIJE
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void RegisterAdmin(string username, string password)
        {
            var (hash, salt) = PasswordHelper.HashPassword(password);

            string connectionString = "YourConnectionStringHere";
            string query = "INSERT INTO Users (Username, PasswordHash, Salt, IsAdmin) VALUES (@Username, @PasswordHash, @Salt, @IsAdmin)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", hash);
                command.Parameters.AddWithValue("@Salt", salt);
                command.Parameters.AddWithValue("@IsAdmin", 1); // Set admin flag

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public bool IsAdmin(string username)
        {
            string connectionString = "YourConnectionStringHere";
            string query = "SELECT IsAdmin FROM Users WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetBoolean(0); // Return IsAdmin flag
                    }
                }
            }
            return false;
        }
        public void UpdateUser(string username, string newUsername, string newPassword)
        {
            var (hash, salt) = PasswordHelper.HashPassword(newPassword);

            string connectionString = "YourConnectionStringHere";
            string query = "UPDATE Users SET Username = @NewUsername, PasswordHash = @PasswordHash, Salt = @Salt WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NewUsername", newUsername);
                command.Parameters.AddWithValue("@PasswordHash", hash);
                command.Parameters.AddWithValue("@Salt", salt);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void DeleteUser(string username)
        {
            string connectionString = "YourConnectionStringHere";
            string query = "DELETE FROM Users WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void PerformAdminTask(string adminUsername, Action adminTask)
        {
            if (!IsAdmin(adminUsername))
            {
                throw new UnauthorizedAccessException("You do not have permission to perform this action.");
            }

            // Execute admin task
            adminTask();
        }
        //PerformAdminTask("adminUsername", () => DeleteUser("targetUsername"));

    }
}
