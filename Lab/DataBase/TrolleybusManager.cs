using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab.DataBase
{
    internal class Trolleybus
    {
        public int Number { get; set; }
        public int Year { get; set; }
        public string? Brand { get; set; }
        public int Seats { get; set; }
        public int? RouteId { get; set; }

        public override string ToString()
        {
            string routeStr = RouteId.HasValue ? RouteId.Value.ToString() : "Не призначено (NULL)";
            return $"Тролейбус №{Number} | Марка: {Brand} ({Year} р.) | Місць: {Seats} | Маршрут: {routeStr}";
        }
    }
    internal class TrolleybusManager
    {
        private readonly string _connectionString;

        public TrolleybusManager(string dbFilePath)
        {
            _connectionString = $"Data Source={dbFilePath};";
        }

        public void Add(Trolleybus trolleybus)
        {
            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Trolleybus VALUES (@Number, @Year, @Brand, @Seats, @RouteId)";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Number", trolleybus.Number);
                    cmd.Parameters.AddWithValue("@Year", trolleybus.Year);
                    cmd.Parameters.AddWithValue("@Brand", trolleybus.Brand);
                    cmd.Parameters.AddWithValue("@Seats", trolleybus.Seats);
                    cmd.Parameters.AddWithValue("@RouteId", trolleybus.RouteId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Trolleybus trolleybus, int oldNumber)
        {
            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE Trolleybus SET Number = @Number, Year = @Year, Brand = @Brand, Seats = @Seats, RouteId = @RouteId WHERE Number = @oldNumber";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Number", trolleybus.Number);
                    cmd.Parameters.AddWithValue("@Year", trolleybus.Year);
                    cmd.Parameters.AddWithValue("@Brand", trolleybus.Brand);
                    cmd.Parameters.AddWithValue("@Seats", trolleybus.Seats);
                    cmd.Parameters.AddWithValue("@RouteId", trolleybus.RouteId);
                    cmd.Parameters.AddWithValue("@oldNumber", oldNumber);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int Number)
        {
            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM Trolleybus WHERE Number = @Number";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Number", Number);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool CheckByNumber(int Number)
        {
            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Trolleybus WHERE Number = @id";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", Number);

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public Trolleybus GetByNumber(int Number)
        {
            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Trolleybus WHERE Number = @id";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", Number);

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Trolleybus
                            {
                                Number = Convert.ToInt32(reader["Number"]),
                                Year = Convert.ToInt32(reader["Year"]),
                                Brand = reader["Brand"] != DBNull.Value ? reader["Brand"].ToString() : null,
                                Seats = Convert.ToInt32(reader["Seats"]),
                                RouteId = reader["RouteID"] != DBNull.Value ? Convert.ToInt32(reader["RouteID"]) : (int?)null
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<Trolleybus> GetAll()
        {
            List<Trolleybus> trolleybuses = new List<Trolleybus>();

            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Trolleybus ORDER BY Number";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            trolleybuses.Add(new Trolleybus
                            {
                                Number = Convert.ToInt32(reader["Number"]),
                                Year = Convert.ToInt32(reader["Year"]),
                                Brand = reader["Brand"] != DBNull.Value ? reader["Brand"].ToString() : null,
                                Seats = Convert.ToInt32(reader["Seats"]),
                                RouteId = reader["RouteID"] != DBNull.Value ? Convert.ToInt32(reader["RouteID"]) : (int?)null
                            });
                        }
                    }
                }
            }
            return trolleybuses;
        }
    }
}

