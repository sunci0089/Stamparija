using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using MySql.Data.MySqlClient;

    public class ConnectionPool
    {
        private static readonly string ConfigFileName = "ConnectionPool.config";

        private string jdbcURL;
        private string username;
        private string password;
        private int preconnectCount;
        private int maxIdleConnections;
        private int maxConnections;

        private int connectCount;
        private List<MySqlConnection> usedConnections;
        private List<MySqlConnection> freeConnections;

        private static ConnectionPool instance;
        private static readonly object lockObject = new object();

        public static ConnectionPool GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                        instance = new ConnectionPool();
                }
            }
            return instance;
        }

        private ConnectionPool()
        {
            ReadConfiguration();
            freeConnections = new List<
                MySqlConnection>();
            usedConnections = new List<MySqlConnection>();

            try
            {
                for (int i = 0; i < preconnectCount; i++)
                {
                    var conn = new MySqlConnection(jdbcURL);
                    conn.Open();
                    freeConnections.Add(conn);
                }
                connectCount = preconnectCount;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void ReadConfiguration()
        {
            if (!File.Exists(ConfigFileName))
                throw new FileNotFoundException($"Configuration file '{ConfigFileName}' not found.");

            var config = new Dictionary<string, string>();
            foreach (var line in File.ReadLines(ConfigFileName))
            {
                var parts = line.Split('=');
                if (parts.Length == 2)
                    config[parts[0].Trim()] = parts[1].Trim();
            }

            jdbcURL = config["jdbcURL"];
            username = config["username"];
            password = config["password"];
            preconnectCount = int.Parse(config.TryGetValue("preconnectCount", out var pre) ? pre : "0");
            maxIdleConnections = int.Parse(config.TryGetValue("maxIdleConnections", out var maxIdle) ? maxIdle : "10");
            maxConnections = int.Parse(config.TryGetValue("maxConnections", out var max) ? max : "10");
        }

        public MySqlConnection CheckOut()
        {
            lock (lockObject)
            {
                if (freeConnections.Count > 0)
                {
                    var conn = freeConnections[0];
                    freeConnections.RemoveAt(0);
                    usedConnections.Add(conn);
                    return conn;
                }

                if (connectCount < maxConnections)
                {
                    var conn = new MySqlConnection(jdbcURL);
                    conn.Open();
                    usedConnections.Add(conn);
                    connectCount++;
                    return conn;
                }

                while (freeConnections.Count == 0)
                {
                    Monitor.Wait(lockObject);
                }

                var connection = freeConnections[0];
                freeConnections.RemoveAt(0);
                usedConnections.Add(connection);
                return connection;
            }
        }

        public void CheckIn(MySqlConnection conn)
        {
            if (conn == null)
                return;

            lock (lockObject)
            {
                if (usedConnections.Remove(conn))
                {
                    freeConnections.Add(conn);

                    while (freeConnections.Count > maxIdleConnections)
                    {
                        var lastIndex = freeConnections.Count - 1;
                        var toClose = freeConnections[lastIndex];
                        freeConnections.RemoveAt(lastIndex);

                        try
                        {
                            toClose.Close();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }

                    Monitor.Pulse(lockObject);
                }
            }
        }
    }
