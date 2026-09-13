using System;
using Microsoft.Data.Sqlite;

namespace Lab.Test
{
    internal class TestBD
    {
        public static void InitializeTestData(string dbFilePath)
        {
            string connectionString = $"Data Source={dbFilePath};Foreign Keys=True;";

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                // Очищення поточних даних (спочатку дочірня таблиця Trolleybus через зовнішній ключ, потім Route)
                using (var clearCmd = conn.CreateCommand())
                {
                    clearCmd.CommandText = "DELETE FROM Trolleybus; DELETE FROM Route;";
                    clearCmd.ExecuteNonQuery();
                }

                // Масив із 5 тестових маршрутів
                var routes = new[]
                {
                    new { Id = 1, StopCount = 5, Distance = 4.5, StartStop = "Центр" },
                    new { Id = 2, StopCount = 8, Distance = 7.2, StartStop = "Вокзал" },
                    new { Id = 3, StopCount = 6, Distance = 5.0, StartStop = "Ринок" },
                    new { Id = 4, StopCount = 10, Distance = 12.3, StartStop = "Парк" },
                    new { Id = 5, StopCount = 4, Distance = 3.1, StartStop = "Університет" }
                };

                // Вставка маршрутів
                foreach (var r in routes)
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO Route (ID, StopCount, Distance, StartStop) VALUES (@id, @stops, @dist, @start)";
                        cmd.Parameters.AddWithValue("@id", r.Id);
                        cmd.Parameters.AddWithValue("@stops", r.StopCount);
                        cmd.Parameters.AddWithValue("@dist", r.Distance);
                        cmd.Parameters.AddWithValue("@start", r.StartStop);
                        cmd.ExecuteNonQuery();
                    }
                }

                Random random = new Random();
                int trolleybusNumber = 101;
                string[] brands = { "Богдан", "Електрон", "ЛАЗ", "Skoda", "МАЗ" };

                foreach (var r in routes)
                {
                    int trolleybusCount = random.Next(2, 5); 

                    for (int i = 0; i < trolleybusCount; i++)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "INSERT INTO Trolleybus (Number, Year, Brand, Seats, RouteID) VALUES (@num, @year, @brand, @seats, @routeId)";
                            cmd.Parameters.AddWithValue("@num", trolleybusNumber++);
                            cmd.Parameters.AddWithValue("@year", random.Next(2010, 2025));
                            cmd.Parameters.AddWithValue("@brand", brands[random.Next(brands.Length)]);
                            cmd.Parameters.AddWithValue("@seats", random.Next(22, 42));
                            cmd.Parameters.AddWithValue("@routeId", r.Id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}