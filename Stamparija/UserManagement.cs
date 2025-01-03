using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Stamparija.DTO;
using Org.BouncyCastle.Crypto.Digests;
using System.IO;
using Stamparija.theme;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Text.Json;

namespace Stamparija
{
    public class UserManagement
    {
        public static Zaposleni zaposleni { get; set; } //todo Da li private?
        public static UserSettings userSettings { get; set; }

        private static string SettingsFilePath { get; set; }


        public static void LoadUserSettings(string username)
        {

            SettingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings", username + ".json");
            //deserialize

            if (!File.Exists(SettingsFilePath))
            {
                //create new user settings
                userSettings = new UserSettings();
                userSettings.FontSize = "small";
                userSettings.Theme = "light";
                userSettings.Language = "eng";
                File.WriteAllText(SettingsFilePath, JsonSerializer.Serialize(userSettings));
            }
            else
            {
                string json = File.ReadAllText(SettingsFilePath);
                userSettings = JsonSerializer.Deserialize<UserSettings>(json);
            }

            Settings.Default.CurrentUser = username;
            Settings.Default.FontSize = userSettings.FontSize;
            Settings.Default.Theme = userSettings.Theme;
            Settings.Default.Language = userSettings.Language;
            Settings.Default.Save();

            AppTheme.LoadTheme();
        }

        public static void SaveUserSettings()
        {
            string username = Settings.Default.CurrentUser;
            if (string.IsNullOrEmpty(username) || userSettings==null) LoadUserSettings("default");

            Settings.Default.FontSize = userSettings.FontSize;

            Settings.Default.Theme = userSettings.Theme;

            Settings.Default.Language = userSettings.Language;

            Settings.Default.Save();

            AppTheme.LoadTheme();
        }
        // Using BouncyCastle for password hashing
        public static (string Hash, string Salt) HashPassword(string password)
        {
            // Generate a salt
            byte[] saltBytes = new byte[16];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            // Combine password and salt
            string saltedPassword = password + salt;

            // Hash the salted password using BouncyCastle's SHA256
            var sha256Digest = new Sha256Digest();
            byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            sha256Digest.BlockUpdate(saltedPasswordBytes, 0, saltedPasswordBytes.Length);
            byte[] hashBytes = new byte[sha256Digest.GetDigestSize()];
            sha256Digest.DoFinal(hashBytes, 0);

            string hash = Convert.ToBase64String(hashBytes);
            return (hash, salt);
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            string saltedPassword = password + storedSalt;

            // Hash the salted password using BouncyCastle's SHA256
            var sha256Digest = new Org.BouncyCastle.Crypto.Digests.Sha256Digest();
            byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            sha256Digest.BlockUpdate(saltedPasswordBytes, 0, saltedPasswordBytes.Length);
            byte[] hashBytes = new byte[sha256Digest.GetDigestSize()];
            sha256Digest.DoFinal(hashBytes, 0);

            string computedHash = Convert.ToBase64String(hashBytes);
            return computedHash == storedHash;
        }

        public void RegisterUser(string username, string password)
        {
            var (hash, salt) = HashPassword(password);

            string connectionString = "YourConnectionStringHere";
            string query = "INSERT INTO Users (Username, PasswordHash, Salt) VALUES (@Username, @PasswordHash, @Salt)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
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

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedHash = reader.GetString(0);
                        string storedSalt = reader.GetString(1);

                        return VerifyPassword(password, storedHash, storedSalt);
                    }
                }
            }
            return false;
        }

        // ADMINISTRATORSKE FUNKCIJE
        public void RegisterAdmin(string username, string password)
        {
            var (hash, salt) = HashPassword(password);

            string connectionString = "YourConnectionStringHere";
            string query = "INSERT INTO Users (Username, PasswordHash, Salt, IsAdmin) VALUES (@Username, @PasswordHash, @Salt, @IsAdmin)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
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

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
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
            var (hash, salt) = HashPassword(newPassword);

            string connectionString = "YourConnectionStringHere";
            string query = "UPDATE Users SET Username = @NewUsername, PasswordHash = @PasswordHash, Salt = @Salt WHERE Username = @Username";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
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

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
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
    }

}
