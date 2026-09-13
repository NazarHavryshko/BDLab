using System;
using System.Collections.Generic;
using System.Text;
using Lab.DataBase;
using System.IO;
using Lab.Verification;
using Lab.Test;

namespace Lab.menu
{
    internal class MainMenu
    {
        public static void Start(string dbPath)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Робота з базою: {dbPath}");
                Console.WriteLine("\n=== ГОЛОВНЕ МЕНЮ ===");
                Console.WriteLine("1. Робота з маршрутами");
                Console.WriteLine("2. Робота з тролейбусами");
                Console.WriteLine("0. Вихід");
                Console.Write("Ваш вибір: ");

                int choice = MenuVerification.GetValidChoice();

                switch (choice)
                {
                    case 1:
                        RouteMenu.Start(dbPath);
                        break;
                    case 2:
                        TrolleybusMenu.Start(dbPath);
                        break;
                    case 9:
                        TestBD.InitializeTestData(dbPath);
                        break;
                    case 0:
                        Console.WriteLine("Програму завершено.");
                        return;
                    default:
                        Console.WriteLine("Невірний вибір. Натисніть Enter...");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}
