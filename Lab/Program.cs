using System;
using System.IO;
using System.Collections.Generic;
using Lab.menu;
using Lab.DataBase;

namespace Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string currentDirectory = Directory.GetCurrentDirectory();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ГОЛОВНЕ МЕНЮ БАЗИ ДАНИХ ===");
                Console.WriteLine("1. Відкрити існуючу базу даних");
                Console.WriteLine("2. Створити нову базу даних");
                Console.WriteLine("0. Вихід");
                Console.Write("Виберіть дію: ");

                string choice = Console.ReadLine()?.Trim();

                if (choice == "0")
                {
                    return;
                }
                else if (choice == "1")
                {
                    string[] dbFiles = Directory.GetFiles(currentDirectory, "*.db");

                    if (dbFiles.Length == 0)
                    {
                        Console.WriteLine("\n[Помилка] У папці з програмою не знайдено жодного файлу бази даних (*.db)!");
                        Console.WriteLine("Натисніть Enter, щоб повернутися в меню...");
                        Console.ReadLine();
                        continue;
                    }

                    Console.WriteLine("\n--- ДОСТУПНІ БАЗИ ДАНИХ ---");
                    for (int i = 0; i < dbFiles.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {Path.GetFileName(dbFiles[i])}");
                    }

                    Console.Write("\nВиберіть номер бази даних: ");
                    if (int.TryParse(Console.ReadLine(), out int dbIndex) && dbIndex >= 1 && dbIndex <= dbFiles.Length)
                    {
                        string selectedDbPath = dbFiles[dbIndex - 1];
                        MainMenu.Start(selectedDbPath);
                    }
                    else
                    {
                        Console.WriteLine("\n[Помилка] Невірний вибір. Натисніть Enter для продовження...");
                        Console.ReadLine();
                    }
                }
                else if (choice == "2")
                {
                    Console.Write("\nВведіть назву нового файлу бази даних (наприклад, my_base або base.db): ");
                    string fileName = Console.ReadLine()?.Trim();

                    if (string.IsNullOrEmpty(fileName))
                    {
                        Console.WriteLine("\n[Помилка] Назва не може бути порожньою!");
                        Console.WriteLine("Натисніть Enter для продовження...");
                        Console.ReadLine();
                        continue;
                    }

                    // Якщо користувач не вказав розширення .db, додаємо його автоматично
                    if (!fileName.EndsWith(".db", StringComparison.OrdinalIgnoreCase))
                    {
                        fileName += ".db";
                    }

                    string newDbFilePath = Path.Combine(currentDirectory, fileName);

                    if (File.Exists(newDbFilePath))
                    {
                        Console.Write($"\nФайл '{fileName}' вже існує. Бажаєте перезаписати його? (y/n): ");
                        if (Console.ReadLine()?.Trim().ToLower() != "y")
                        {
                            continue;
                        }
                        File.Delete(newDbFilePath);
                    }

                    // Створюємо нову базу через ваш клас ініціалізації
                    DatabaseInitializer.CreateDatabaseAndSchema(newDbFilePath);
                    Console.WriteLine("Натисніть Enter, щоб перейти до роботи з новою базою...");
                    Console.ReadLine();

                    MainMenu.Start(newDbFilePath);
                }
                else
                {
                    Console.WriteLine("\n[Помилка] Невірний вибір. Натисніть Enter для продовження...");
                    Console.ReadLine();
                }
            }
        }
    }
}