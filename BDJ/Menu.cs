using BDJ.Models;
using BDJ.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ
{
    internal class Menu
    {

        private User? _currentUser = null;
        private readonly TrainSystemContext _trainSystemContext;
        private readonly TicketService _ticketService;
        private readonly BookingService _bookingService;
        private readonly UserService _userService;
        private readonly TrainService _trainService;

        public Menu()
        {
            _trainSystemContext = new TrainSystemContext();
            _ticketService = new TicketService(_trainSystemContext);
            _bookingService = new BookingService(_trainSystemContext);
            _userService = new UserService(_trainSystemContext);
            _trainService = new TrainService(_trainSystemContext);
        }

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

        public void DisplayLoginMenu()
        {
            while (_currentUser == null)
            {
                Action printMenu = () =>
                {
                    Console.Clear();
                    Console.WriteLine("Hello, please select an option:");
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Register");
                    Console.WriteLine("0. Exit");
                };

                printMenu();
                int option = GetUserChoice(printMenu);
                switch (option)
                {
                    case 1:
                        LoginUser();
                        break;
                    case 2:
                        RegisterUser();
                        break;
                    case 0:
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid option selected. Please try again.");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
        public void DisplayMainMenu()
        {
            while (_currentUser != null)
            {
                Action printMenu = () =>
                {
                    var name = _currentUser.Name.Length > 0 ? _currentUser.Name : "";
                    Console.Clear();
                    Console.WriteLine($"Hello {name}, please select an option:");

                    Console.WriteLine("1. Print trains for today");
                    Console.WriteLine("2. Search train by date");
                    Console.WriteLine("3. Search train by destiantion");
                    Console.WriteLine("4. Search train by date and destiantion");

                    Console.WriteLine("5. Book ticket");
                    Console.WriteLine("6. Show all tickets");
                    Console.WriteLine("7. Show all active tickets");
                    Console.WriteLine("8. Cancel ticket");
                    Console.WriteLine("9. Update ticket's date");
                    Console.WriteLine("10. Profile");

                    if (_currentUser.IsAdmin)
                    {
                        Console.WriteLine("11. Mock trains");
                        Console.WriteLine("12. Show all user profiles");
                        Console.WriteLine("13. Search user profile by name");
                        Console.WriteLine("14. Add new user profile");
                        Console.WriteLine("15. Update user's profile");
                        Console.WriteLine("16. give discount card");
                        Console.WriteLine("17. book ticket to user");
                    }

                    Console.WriteLine("0. Exit");
                };

                printMenu();
                int option = GetUserChoice(printMenu);
                switch (option)
                {
                    case 1:
                        _trainService.PrintDailyTrains();
                        break;
                    case 2:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 3:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 4:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 5:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 6:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 7:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 8:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 9:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 10:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 11:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 12:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 13:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 14:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 15:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 16:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 17:
                        Console.WriteLine(_currentUser.Name);
                        break;
                    case 0:
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid option selected. Please try again.");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void Main()
        {
            DisplayLoginMenu();
            DisplayMainMenu();
        }

        private void RegisterUser()
        {
            string? name = null;
            string? password = null;
            Console.WriteLine("Enter Name: ");
            name = Console.ReadLine();
            Console.WriteLine("Enter Age: ");
            string? numinput = Console.ReadLine();
            int.TryParse(numinput, out int age);
            Console.WriteLine("Enter Password: ");
            password = Console.ReadLine();

            if (age > 0 && name != null && password != null)
            {
                var user = _userService.Register(name, password, age);
                if (user != null)
                {
                    _currentUser = user;
                    Console.WriteLine("User Was Registered!");
                }
            }
            else
            {
                Console.WriteLine("Invalid Input. Try Again!");
            }
        }
        private void LoginUser()
        {
            string? name = null;
            string? password = null;
            Console.WriteLine("Enter Name: ");
            name = Console.ReadLine();
            Console.WriteLine("Enter Password: ");
            password = Console.ReadLine();

            if (name != null && password != null)
            {
                var user = _userService.Login(name, password);
                if (user != null)
                {
                    _currentUser = user;
                    Console.WriteLine("User Login was Successful!");
                }
            }
            else
            {
                Console.WriteLine("Invalid Input. Try Again!");
            }
        }
        private int GetUserChoice(Action printMenu)
        {
            int choice = -1;
            while (choice < 0)
            {
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out choice))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }
                printMenu();
            }
            return choice;
        }
    }
}
