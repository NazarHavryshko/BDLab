using System;
using System.Collections.Generic;
using Lab.DataBase;
using Lab.Verification;

namespace Lab.menu
{
    internal class TrolleybusMenu
    {
        public static void Start(string dbPath)
        {
            TrolleybusManager trolleybusManager = new TrolleybusManager(dbPath);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n--- МЕНЮ: ТРОЛЕЙБУСИ ---");
                Console.WriteLine("1. Вивести всі тролейбуси");
                Console.WriteLine("2. Додати тролейбус");
                Console.WriteLine("3. Оновити тролейбус");
                Console.WriteLine("4. Видалити тролейбус");
                Console.WriteLine("0. Повернутися назад");
                Console.Write("Ваш вибір: ");

                int choice = MenuVerification.GetValidChoice();

                switch (choice)
                {
                    case 1:
                        ShowAllTrolleybuses(trolleybusManager);
                        break;
                    case 2:
                        AddTrolleybus(trolleybusManager);
                        break;
                    case 3:
                        UpdateTrolleybus(trolleybusManager);
                        break;
                    case 4:
                        DeleteTrolleybus(trolleybusManager);
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Невірний вибір. Натисніть Enter...");
                        Console.ReadLine();
                        break;
                }

                Console.WriteLine("\nНатисніть Enter для продовження...");
                Console.ReadLine();
            }
        }

        private static void ShowAllTrolleybuses(TrolleybusManager tm)
        {
            Console.WriteLine("\n--- СПИСОК ТРОЛЕЙБУСІВ ---");
            var trolleybuses = tm.GetAll();

            if (trolleybuses.Count == 0)
            {
                Console.WriteLine("База даних тролейбусів порожня.");
                return;
            }

            foreach (var t in trolleybuses)
            {
                Console.WriteLine(t.ToString());
            }
        }

        private static void AddTrolleybus(TrolleybusManager tm)
        {
            Console.WriteLine("\n--- ДОДАВАННЯ ТРОЛЕЙБУСА ---");

            int number = MenuVerification.GetInt("Введіть бортовий номер тролейбуса: ");
            while (tm.CheckByNumber(number))
            {
                Console.WriteLine("Тролейбус з таким номером вже існує. Спробуйте ще раз.");
                number = MenuVerification.GetInt("Введіть бортовий номер тролейбуса: ");
            }

            int year = MenuVerification.GetInt("Введіть рік випуску: ");
            while (year < 1980 || year > DateTime.Now.Year)
            {
                Console.WriteLine("Рік випуску має бути більше ніж 1980 і не більший за поточний. Спробуйте ще раз.");
                year = MenuVerification.GetInt("Введіть рік випуску: ");
            }

            Console.Write("Введіть марку тролейбуса: ");
            string brand = Console.ReadLine().Trim();

            int seats = MenuVerification.GetInt("Введіть кількість місць: ");

            // Для RouteId робимо підтримку Null, якщо користувач просто натисне Enter
            Console.Write("Введіть ID маршруту (або натисніть Enter, якщо не призначено): ");

            string routeInput = Console.ReadLine().Trim();
            int? routeId = null;

            if (!string.IsNullOrEmpty(routeInput) && int.TryParse(routeInput, out int rId))
            {
                routeId = rId;
            }

            Trolleybus newTrolleybus = new Trolleybus
            {
                Number = number,
                Year = year,
                Brand = brand,
                Seats = seats,
                RouteId = routeId
            };

            tm.Add(newTrolleybus);
            Console.WriteLine("Тролейбус успішно додано!");
        }

        private static void UpdateTrolleybus(TrolleybusManager tm)
        {
            Console.WriteLine("\n--- ОНОВЛЕННЯ ТРОЛЕЙБУСА ---");

            int oldNumber = MenuVerification.GetInt("Введіть номер тролейбуса, який хочете оновити: ");

            // Отримуємо існуючий об'єкт з бази через GetByNumber
            Trolleybus existingTrolleybus = tm.GetByNumber(oldNumber);
            while (!tm.CheckByNumber(oldNumber))
            {
                Console.WriteLine("Тролейбус з таким номером не існує. Спробуйте ще раз.");
                oldNumber = MenuVerification.GetInt("Введіть номер тролейбуса, який хочете оновити: ");
                existingTrolleybus = tm.GetByNumber(oldNumber);
            }

            Console.WriteLine($"\nЗнайдено тролейбус: {existingTrolleybus}");
            Console.WriteLine("Якщо ви не хочете змінювати певне поле, просто натисніть Enter (для чисел введіть поточне значення або залиште логіку за потребою).");

            // Номер
            int newNumber = MenuVerification.GetInt($"Введіть НОВИЙ номер (поточний: {existingTrolleybus.Number}): ");
            while (newNumber != oldNumber && tm.GetByNumber(newNumber) != null)
            {
                Console.WriteLine("Тролейбус з таким новим номером вже існує. Спробуйте ще раз.");
                newNumber = MenuVerification.GetInt("Введіть НОВИЙ номер тролейбуса: ");
            }

            // Рік випуску
            int year = MenuVerification.GetInt($"Введіть НОВИЙ рік випуску (поточний: {existingTrolleybus.Year}): ");
            while (year < 1980 || year > DateTime.Now.Year)
            {
                Console.WriteLine("Рік випуску має бути більше ніж 1980 і не більший за поточний. Спробуйте ще раз.");
                year = MenuVerification.GetInt("Введіть рік випуску: ");
            }

            // Марка
            Console.Write($"Введіть НОВУ марку (поточна: {existingTrolleybus.Brand}): ");
            string brandInput = Console.ReadLine().Trim();
            string brand = string.IsNullOrEmpty(brandInput) ? existingTrolleybus.Brand : brandInput;

            // Кількість місць
            int seats = MenuVerification.GetInt($"Введіть НОВУ кількість місць (поточна: {existingTrolleybus.Seats}): ");

            // ID маршруту
            Console.Write($"Введіть НОВИЙ ID маршруту (поточний: {(existingTrolleybus.RouteId.HasValue ? existingTrolleybus.RouteId.Value.ToString() : "NULL")}, або Enter щоб залишити без змін): ");
            string routeInput = Console.ReadLine().Trim();
            int? routeId = existingTrolleybus.RouteId;

            if (!string.IsNullOrEmpty(routeInput))
            {
                if (int.TryParse(routeInput, out int rId))
                {
                    routeId = rId;
                }
            }

            // Оновлюємо поля існуючого об'єкта
            existingTrolleybus.Number = newNumber;
            existingTrolleybus.Year = year;
            existingTrolleybus.Brand = brand;
            existingTrolleybus.Seats = seats;
            existingTrolleybus.RouteId = routeId;

            // Передаємо повністю оновлений об'єкт на збереження
            tm.Update(existingTrolleybus, oldNumber);
            Console.WriteLine("Тролейбус успішно оновлено!");
        }

        private static void DeleteTrolleybus(TrolleybusManager tm)
        {
            Console.WriteLine("\n--- ВИДАЛЕННЯ ТРОЛЕЙБУСА ---");

            int number = MenuVerification.GetInt("Введіть номер тролейбуса для видалення: ");
            while (!tm.CheckByNumber(number))
            {
                Console.WriteLine("Тролейбус з таким номером не знайдено. Спробуйте ще раз.");
                number = MenuVerification.GetInt("Введіть номер тролейбуса для видалення: ");
            }

            tm.Delete(number);
            Console.WriteLine("Тролейбус успішно видалено!");
        }
    }
}