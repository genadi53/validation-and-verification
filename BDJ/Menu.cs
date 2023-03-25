using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ
{
    internal class Menu
    {
        public static void DisplayFancyMenu()
        {
            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.ResetColor();
            Console.WriteLine("\nUse ⬆️  and ⬇️  to navigate and press \u001b[32mEnter/Return\u001b[0m to select:");
            (int left, int top) = Console.GetCursorPosition();
            var option = 1;
            var decorator = "✅ \u001b[32m";
            ConsoleKeyInfo key;
            bool isSelected = false;

            while (!isSelected)
            {
                Console.SetCursorPosition(left, top);

                Console.WriteLine($"{(option == 1 ? decorator : "   ")}Register\u001b[0m");
                Console.WriteLine($"{(option == 2 ? decorator : "   ")}Login\u001b[0m");
                Console.WriteLine($"{(option == 3 ? decorator : "   ")}Exit\u001b[0m");

                key = Console.ReadKey(false);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        option = option == 1 ? 3 : option - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        option = option == 3 ? 1 : option + 1;
                        break;

                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                }
            }

            Console.WriteLine($"\n{decorator}You selected Option {option}");

            switch (option)
            {
                case 1:
                    Console.WriteLine($"\n{decorator}You selected Option {option}");
                    break;

                case 2:
                    Console.WriteLine($"\n{decorator}You selected Option {option}");
                    break;

                case 3:
                    Console.WriteLine($"\n{decorator}You selected Option {option}");
                    break;
            }

            Console.ReadLine();
        }

        public static void DisplayMenu()
        {
            // Loop until the user selects the "Exit" option
            while (true)
            {
                // Display the menu options
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Option 1");
                Console.WriteLine("2. Option 2");
                Console.WriteLine("3. Option 3");
                Console.WriteLine("0. Exit");

                // Read the user's input
                string input = Console.ReadLine();

                // Parse the input as an integer
                if (!int.TryParse(input, out int option))
                {
                    // If the input is not a valid integer, display an error message
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                // Check which option was selected
                switch (option)
                {
                    case 1:
                        Console.WriteLine("Option 1 selected.");
                        // Do something for Option 1
                        break;
                    case 2:
                        Console.WriteLine("Option 2 selected.");
                        // Do something for Option 2
                        break;
                    case 3:
                        Console.WriteLine("Option 3 selected.");
                        // Do something for Option 3
                        break;
                    case 0:
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid option selected. Please try again.");
                        break;
                }

                // Wait for the user to press a key before clearing the console
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
