using BDJ.Models;
using BDJ.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ
{
    internal class Menu
    {

        private User? _currentUser = null;
        private readonly TrainSystemContext _trainSystemContext;
        private readonly BookingService _bookingService;
        private readonly UserService _userService;
        private readonly TrainService _trainService;

        public Menu()
        {
            _trainSystemContext = new TrainSystemContext();
            _bookingService = new BookingService(_trainSystemContext);
            _userService = new UserService(_trainSystemContext);
            _trainService = new TrainService(_trainSystemContext);
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

                int option = Menu.GetUserChoice(printMenu);
                switch (option)
                {
                    case 1:
                        {
                            var user = LoginUser();
                            _currentUser = user;
                            break;
                        }
                    case 2:
                        {
                            var user = RegisterUser();
                            _currentUser = user;
                            break;
                        }
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
                    Console.WriteLine("2. Search trains by date");
                    Console.WriteLine("3. Search trains by destiantion");
                    Console.WriteLine("4. Search trains by date and destiantion");

                    Console.WriteLine("5. Book ticket");
                    Console.WriteLine("6. Show all booked tickets");
                    Console.WriteLine("7. Show all Active tickets");
                    Console.WriteLine("8. Show all Canceled tickets");
                    Console.WriteLine("9. Cancel ticket");
                    Console.WriteLine("10. Update ticket's date");
                    Console.WriteLine("11. Profile");

                    if (_currentUser.IsAdmin)
                    {
                        Console.WriteLine("12. Mock trains");
                        Console.WriteLine("13. Show all user profiles");
                        Console.WriteLine("14. Search user profile by name");
                        Console.WriteLine("15. Add new user profile");
                        Console.WriteLine("16. Update user's profile");
                        Console.WriteLine("17. Add discount card to user");
                        Console.WriteLine("18. Print all bookings");
                    }

                    Console.WriteLine("0. Exit");
                };

                printMenu();
                int option = Menu.GetUserChoice(printMenu);
                switch (option)
                {
                    case 1:
                        _trainService.PrintDailyTrains();
                        break;
                    case 2:
                        {
                            DateTime date = Time.GetDateInput();
                            var trains = _trainService.SearchTrainByDepartureDate(date);
                            TrainService.PrintGivenTrains(trains);
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("Enter train destination: ");
                            string? destination = Console.ReadLine();

                            if (String.IsNullOrEmpty(destination))
                            {
                                Console.WriteLine("Destiantion can not be empty!");
                                break;
                            }

                            var trains = _trainService.SearchTrainByDestination(destination);
                            TrainService.PrintGivenTrains(trains);
                            break;
                        }
                    case 4:
                        {
                            Console.WriteLine("Enter train destination: ");
                            string? destination = Console.ReadLine();

                            if (String.IsNullOrEmpty(destination))
                            {
                                Console.WriteLine("Destiantion can not be empty!");
                                break;
                            }
                            DateTime date = Time.GetDateInput();
                            var trains = _trainService.SearchTrainByDateAndDestination(destination, date);
                            TrainService.PrintGivenTrains(trains);
                            break;
                        }
                    case 5:
                        {
                            double price = 10;
                            Console.WriteLine("Enter train departure station: ");
                            string? departure = Console.ReadLine();

                            if (String.IsNullOrEmpty(departure))
                            {
                                Console.WriteLine("Departure station can not be empty!");
                                break;
                            }

                            Console.WriteLine("Enter train destination: ");
                            string? destination = Console.ReadLine();

                            if (String.IsNullOrEmpty(destination))
                            {
                                Console.WriteLine("Destiantion can not be empty!");
                                break;
                            }
                            Console.WriteLine("Enter date for your ticket");
                            DateTime date = Time.GetDateInput();

                            Console.WriteLine("With child: ");
                            bool withChild = false;
                            bool.TryParse(Console.ReadLine(), out withChild);

                            _bookingService.BookTicket(_currentUser, departure, destination, price, date, withChild);
                            break;
                        }
                    case 6:
                        _userService.PrintAllUserTickets(_currentUser);
                        break;
                    case 7:
                        _userService.PrintAllActiveUserTickets(_currentUser);
                        break;
                    case 8:
                        _userService.PrintAllCanceledUserTickets(_currentUser);
                        break;
                    case 9:
                        {

                            Console.WriteLine("Enter train departure station: ");
                            string? departureStation = Console.ReadLine();

                            if (departureStation == null)
                            {
                                Console.WriteLine("Departure station can not be empty!");
                                break;
                            }

                            Console.WriteLine("Enter train destination: ");
                            string? destination = Console.ReadLine();

                            if (destination == null)
                            {
                                Console.WriteLine("Destiantion can not be empty!");
                                break;
                            }

                            DateTime date = Time.GetDateInput();
                            _bookingService.CancelBooking(_currentUser, departureStation, destination, date);
                            break;
                        }
                    case 10:
                        {

                            Console.WriteLine("Enter train departure station: ");
                            string? departureStation = Console.ReadLine();

                            if (departureStation == null)
                            {
                                Console.WriteLine("Departure station can not be empty!");
                                break;
                            }

                            Console.WriteLine("Enter train destination: ");
                            string? destination = Console.ReadLine();

                            if (destination == null)
                            {
                                Console.WriteLine("Destiantion can not be empty!");
                                break;
                            }

                            Console.WriteLine("Enter old date: ");
                            DateTime oldDate = Time.GetDateInput();

                            Console.WriteLine("Enter new date: ");
                            DateTime newDate = Time.GetDateInput();

                            _bookingService.UpdateBookingDate(_currentUser, departureStation, destination, oldDate, newDate);
                            break;
                        }
                    case 11:
                        {
                            string cardtype = CardType.GetCardType(_currentUser.Card);
                            Console.WriteLine($"Logged in as {_currentUser.Name}, and have {cardtype} card");
                            break;
                        }
                    case 12:
                        _trainService.MockDailyTrains();
                        break;
                    case 13:
                        _userService.PrintAllUsers(_currentUser.Id);
                        break;
                    case 14:
                        {
                            string? name = null;
                            Console.WriteLine("Enter user's name: ");
                            name = Console.ReadLine();
                            if (name == null)
                            {
                                Console.WriteLine("Invalid Input. Try Again!");
                                break;
                            }

                            var user = _userService.SearchUserByName(name);
                            if (user == null)
                            {
                                Console.WriteLine("No such user!");
                                break;
                            }
                            _userService.PrintUser(user);
                            break;
                        }
                    case 15:
                        RegisterUser();
                        break;
                    case 16:
                        {
                            var userIdInput = 0;
                            var age = 0;
                            Console.WriteLine("Enter user id: ");
                            if (!int.TryParse(Console.ReadLine(), out userIdInput))
                            {
                                Console.WriteLine("Invalid input. Please enter a number.");
                                break;
                            }

                            Console.WriteLine("Enter new Name");
                            var name = Console.ReadLine();

                            Console.WriteLine("Enter new Age");
                            if (!int.TryParse(Console.ReadLine(), out age))
                            {
                                Console.WriteLine("Invalid input. Please enter a number.");
                                break;
                            }

                            if (userIdInput <= 0 || name == null || age <= 0)
                            {
                                Console.WriteLine("Invalid input! Try Again!");
                                break;
                            }

                            _userService.EditUser(_currentUser.Id, userIdInput, name, age, null);
                            break;
                        }
                    case 17:
                        {

                            Console.WriteLine("Enter user id: ");
                            var userIdInput = 0;
                            if (!int.TryParse(Console.ReadLine(), out userIdInput))
                            {
                                Console.WriteLine("Invalid input. Please enter a number.");
                                break;
                            }

                            Console.WriteLine("What is the type of the card?");
                            var cardTypeInput = Console.ReadLine();

                            if (userIdInput <= 0 || cardTypeInput == null)
                            {
                                Console.WriteLine("Invalid input! Try Again!");
                                break;
                            }

                            DiscountCard? card = null;
                            if (cardTypeInput.ToLower().Equals("family"))
                            {
                                card = new DiscountCard { Type = "family" };
                            }
                            else if (cardTypeInput.ToLower().Equals("senior"))
                            {
                                card = new DiscountCard { Type = "senior" };
                            }
                            else
                            {
                                Console.WriteLine("Invalid card type!");
                            }

                            if (card != null)
                            {
                                _userService.EditUser(_currentUser.Id,
                                    userIdInput, null, null, card);
                            }
                            break;
                        }
                    case 18:
                        _bookingService.PrintAllBookings();
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

        private User? RegisterUser()
        {
            string? name = null;
            string? password = null;

            Console.WriteLine("Enter Name: ");
            name = Console.ReadLine();

            Console.WriteLine("Enter Age: ");
            string? numinput = Console.ReadLine();
            if (!int.TryParse(numinput, out int age))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input. Please enter a number.");
                return null;
            }

            Console.WriteLine("Enter Password: ");
            password = Console.ReadLine();

            if (age > 0 && name != null && password != null)
            {
                var user = _userService.Register(name, password, age);
                if (user != null)
                {
                    Console.WriteLine("User Was Registered!");
                    return user;
                }
            }
            else
            {
                Console.WriteLine("Invalid Input. Try Again!");
                return null;
            }
            return null;
        }

        private User? LoginUser()
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
                    Console.WriteLine("User Login was Successful!");
                    return user;
                }
            }
            else
            {
                Console.WriteLine("Invalid Input. Try Again!");
                return null;
            }
            return null;
        }

        private static int GetUserChoice(Action printMenu)
        {
            int choice = -1;
            while (choice < 0)
            {
                printMenu();
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out choice))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
            return choice;
        }
    }
}
