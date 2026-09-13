using System;
using System.Collections.Generic;
using System.Text;
using Lab.DataBase;
using Lab.Verification;


namespace Lab.menu
{
    internal class RouteMenu
    {
        public static void Start(string dbPath)
        {
            RouteManager routeManager = new RouteManager(dbPath);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n--- МЕНЮ: МАРШРУТИ ---");
                Console.WriteLine("1. Вивести всі маршрути");
                Console.WriteLine("2. Додати маршрут");
                Console.WriteLine("3. Оновити маршрут");
                Console.WriteLine("4. Видалити маршрут");
                Console.WriteLine("5. Вивести всі тролейбуси за ID маршруту");
                Console.WriteLine("0. Повернутися назад");
                Console.Write("Ваш вибір: ");

                int choice = MenuVerification.GetValidChoice();

                switch (choice)
                {
                    case 1:
                        ShowAllRoutes(routeManager);
                        break;
                    case 2:
                        AddRoute(routeManager);
                        break;
                    case 3:
                        UpdateRoute(routeManager);
                        break;
                    case 4:
                        DeleteRoute(routeManager);
                        break;
                    case 5:
                        ShowTrolleybusesByRoute(routeManager);
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

        private static void ShowAllRoutes(RouteManager rm)
        {
            Console.WriteLine("\n--- СПИСОК МАРШРУТІВ ---");
            // Передбачається, що в RouteManager є метод GetAllRoutes(), який повертає List<Route>
            var routes = rm.GetAll();

            if (routes.Count == 0)
            {
                Console.WriteLine("База даних маршрутів порожня.");
                return;
            }

            foreach (var route in routes)
            {
                // Передбачається, що в класі Route перевизначено метод ToString()
                Console.WriteLine(route.ToString());
            }
        }

        private static void AddRoute(RouteManager rm)
        {
            Console.WriteLine("\n--- ДОДАВАННЯ МАРШРУТУ ---");

            int id = MenuVerification.GetInt("Введіть ID маршруту: ");
            while (rm.CheckById(id))
            {
                Console.WriteLine("Маршрут з таким ID вже існує. Спробуйте ще раз.");
                id = MenuVerification.GetInt("Введіть ID маршруту: ");
            }

            int stopCount = MenuVerification.GetInt("Введіть кількість зупинок: ");

            double distance = MenuVerification.GetDouble("Введіть відстань (км): ");

            Console.Write("Введіть початкову зупинку: ");
            string startStop = Console.ReadLine().Trim();
            while (rm.CheckByStartStop(startStop))
            {
                Console.WriteLine("Маршрут з такою початковою зупинкою вже існує. Спробуйте ще раз.");
                Console.Write("Введіть початкову зупинку: "); 
                startStop = Console.ReadLine().Trim();
            }
            
            Route newRoute = new Route
            {
                Id = id,
                StopCount = stopCount,
                Distance = distance,
                StartStop = startStop
            };

            rm.Add(newRoute);
            Console.WriteLine("Маршрут успішно додано!");
        }

        private static void UpdateRoute(RouteManager rm)
        {
            ShowAllRoutes(rm);
            Console.WriteLine("\n--- ОНОВЛЕННЯ МАРШРУТУ ЗА ПОЧАТКОВОЮ ЗУПИНКОЮ ---");

            Console.Write("Введіть початкову зупинку маршруту, який хочете оновити (умова пошуку): ");
            string targetStartStop = Console.ReadLine().Trim();
            while (!rm.CheckByStartStop(targetStartStop))
            {
                Console.WriteLine("Маршрут з такою початковою зупинкою не існує. Спробуйте ще раз.");
                Console.Write("Введіть початкову зупинку: ");
                targetStartStop = Console.ReadLine().Trim();
            }


            int newId = MenuVerification.GetInt("Введіть НОВИЙ ID маршруту: ");
            while (rm.CheckById(newId))
            {
                Console.WriteLine("Маршрут з таким ID вже існує. Спробуйте ще раз.");
                newId = MenuVerification.GetInt("Введіть НОВИЙ ID маршрутуу: ");
            }

            int newStopCount = MenuVerification.GetInt("Введіть НОВУ кількість зупинок: ");

            rm.Update(newId, newStopCount, targetStartStop);
        }

        private static void DeleteRoute(RouteManager rm)
        {
            Console.WriteLine("\n--- ВИДАЛЕННЯ МАРШРУТІВ ЗА ДІАПАЗОНОМ ВІДСТАНЕЙ ---");

            double minDist = MenuVerification.GetDouble("Введіть мінімальну відстань (км): ");

            double maxDist = MenuVerification.GetDouble("Введіть максимальну відстань (км): ");


            rm.Delete(minDist, maxDist);
        }

        private static void ShowTrolleybusesByRoute(RouteManager rm)
        {
            Console.WriteLine("\n--- ТРОЛЕЙБУСИ НА МАРШРУТІ ---");
            Console.Write("Введіть ID маршруту: ");
            int routeId = int.Parse(Console.ReadLine());

            var trolleybuses = rm.GetTrolleybusesByRouteId(routeId);

            if (trolleybuses.Count == 0)
            {
                Console.WriteLine("На цьому маршруті немає жодного тролейбуса або маршрут не існує.");
                return;
            }

            foreach (var t in trolleybuses)
            {
                Console.WriteLine(t.ToString());
            }
        }
    }
}
