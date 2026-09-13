using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Lab.DataBase
{
    internal class DatabaseInitializer
    {
        public static void CreateDatabaseAndSchema(string dbFilePath)
        {
            string connectionString = $"Data Source={dbFilePath};Foreign Keys=True;";

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                string schemaSql = @"
                    CREATE TABLE IF NOT EXISTS Route (
                        ID INT PRIMARY KEY NOT NULL, 
                        StopCount INT, 
                        Distance REAL, 
                        StartStop TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Trolleybus (
                        Number INT PRIMARY KEY NOT NULL, 
                        Year INT, 
                        Brand TEXT, 
                        Seats INT, 
                        RouteID INT, 
                        FOREIGN KEY (RouteID) REFERENCES Route(ID)
                    );

                    -- Перевіряємо та створюємо тригери (SQLite не має CREATE TRIGGER IF NOT EXISTS, тому видаляємо перед створенням)
                    DROP TRIGGER IF EXISTS update_route_id_cascade;
                    CREATE TRIGGER update_route_id_cascade AFTER UPDATE OF ID ON Route 
                    BEGIN 
                        UPDATE Trolleybus SET RouteID = NEW.ID WHERE RouteID = OLD.ID; 
                    END;

                    DROP TRIGGER IF EXISTS delete_route;
                    CREATE TRIGGER delete_route BEFORE DELETE ON Route 
                    BEGIN 
                        UPDATE Trolleybus SET RouteID = NULL WHERE RouteID = OLD.ID; 
                    END;
                ";

                using (SqliteCommand cmd = new SqliteCommand(schemaSql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Базу даних та структуру (таблиці, тригери) успішно створено!\n");
        }
    }
}