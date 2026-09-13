using System;
using System.Collections.Generic;
using System.Text;

namespace Lab.Verification
{
    internal class MenuVerification
    {
        public static int GetValidChoice()
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (int.TryParse(input, out int choice))
                {
                    return choice; 
                }

                Console.WriteLine($"\nПомилка вводу! Будь ласка, введіть число.");
                Console.Write("Ваш вибір: ");
            }
        }

        public static int GetInt(string massage)
        {
            Console.Write(massage);
            while (true)
            {
                string input = Console.ReadLine();

                if (int.TryParse(input, out int choice) && choice > 0)
                {
                    return choice; 
                }

                Console.WriteLine($"\nПомилка вводу! Будь ласка, введіть ціле число");
                Console.Write("Введіть повторно: ");
            }
        }

        public static double GetDouble(string massage)
        {
            Console.Write(massage);
            while (true)
            {
                string input = Console.ReadLine();

                if (double.TryParse(input, out double choice) && choice > 0)
                {
                    return choice;
                }

                Console.WriteLine($"\nПомилка вводу! Будь ласка, введіть дробове число (3,бб) число");
                Console.Write("Введіть повторно: ");
            }
        }
    }
}
