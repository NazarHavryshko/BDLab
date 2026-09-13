using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Text;
using Lab.DataBase;

namespace Lab.DataBase
{
    internal class Route
    {
        public int Id { get; set; }
        public int StopCount { get; set; }
        public double Distance { get; set; }
        public string? StartStop { get; set; }

        public override string ToString()
        {
            return $"Маршрут ID: {Id} | Зупинок: {StopCount} | Відстань: {Distance} км | Початкова зупинка: {StartStop}";
        }
    }
    

    internal class RouteManager
    {
        private readonly string _connectionString;

        public RouteManager(string dbFilePath)
        {
            _connectionString = $"Data Source={dbFilePath};";
        }

        public void Add(Route route)
        {
            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Route (ID, StopCount, Distance, StartStop) VALUES (@id, @stops, @distance, @startStop)";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", route.Id);
                    cmd.Parameters.AddWithValue("@stops", route.StopCount);
                    cmd.Parameters.AddWithValue("@distance", route.Distance);
                    cmd.Parameters.AddWithValue("@startStop", route.StartStop);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(int newId, int newStopCount, string targetStartStop)
        {
            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE Route SET ID = @newId, StopCount = @newStopCount WHERE StartStop LIKE @targetStartStop";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@newId", newId);
                    cmd.Parameters.AddWithValue("@newStopCount", newStopCount);
                    cmd.Parameters.AddWithValue("@targetStartStop", targetStartStop);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(double minDist, double maxDist)
        {
            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM Route WHERE Distance BETWEEN @minDist AND @maxDist";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@minDist", minDist);
                    cmd.Parameters.AddWithValue("@maxDist", maxDist);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool CheckById(int Id)
        {
            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Route WHERE ID = @id";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", Id);

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

        public bool CheckByStartStop(string StartStop)
        {
            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Route WHERE StartStop LIKE @StartStop";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@StartStop", StartStop);

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

        public List<Route> GetAll()
        {
            List<Route> routes = new List<Route>();

            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Route order by ID";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            routes.Add(new Route
                            {
                                Id = Convert.ToInt32(reader["ID"]),
                                StopCount = Convert.ToInt32(reader["StopCount"]),
                                Distance = Convert.ToDouble(reader["Distance"]),
                                StartStop = reader["StartStop"].ToString()
                            });
                        }
                    }
                }
            }
            return routes;
        }


        public List<Trolleybus> GetTrolleybusesByRouteId(int routeId)
        {
            List<Trolleybus> trolleybuses = new List<Trolleybus>();

            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT Number, Year, Brand, Seats, RouteID FROM Trolleybus WHERE RouteID = @routeId";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@routeId", routeId);

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            trolleybuses.Add(new Trolleybus
                            {
                                Number = Convert.ToInt32(reader["Number"]),
                                Year = Convert.ToInt32(reader["Year"]),
                                Brand = reader["Brand"].ToString(),
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
