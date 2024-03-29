﻿using BDJ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Services
{
    public class UserService
    {
        private readonly TrainSystemContext _trainSystemContext;
        private readonly TicketService _ticketService;
        private const string delimiter = "#?";

        public UserService(TrainSystemContext trainSystemContext)
        {
            _trainSystemContext = trainSystemContext;
            _ticketService = new TicketService(_trainSystemContext);
        }

        public User? AddUser(string name, int age, string password, bool isAdmin, DiscountCard? card)
        {
            bool isAgeInvalid = age <= 0 && age > 150;
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) || isAgeInvalid)
            {
                Console.WriteLine("User was not registered!");
                return null;
            }

            string hashedPass = GenerateHashedPassword(password);
            var user = new User { Name = name, Age = age, Card = card, Password = hashedPass, IsAdmin = isAdmin };
            _trainSystemContext.Users.Add(user);
            _trainSystemContext.SaveChanges();
            return user;
        }

        public User? SearchUserById(int id)
        {
            return _trainSystemContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public User? SearchUserByName(string name)
        {
            return _trainSystemContext.Users.FirstOrDefault(u => u.Name.Equals(name));
        }

        public User? Login(string name, string password)
        {
            var user = _trainSystemContext.Users.FirstOrDefault(u => u.Name == name);

            if (user == null)
            {
                Console.WriteLine("User was not found");
                return null;
            }

            string? savedSalt = user.Password.Split(delimiter).Last();


            var hashedPassword = HashPassword(password, savedSalt);

            if (user.Password.Equals(String.Concat(hashedPassword, delimiter, savedSalt)))
            {
                return user;
            }
            else
            {
                Console.WriteLine("Try logging again!");
                return null;
            }
        }

        public User? Register(string name, string password, int age)
        {
            var user = AddUser(name: name, password: password, age: age, isAdmin: false, card: null);

            if (user == null)
            {
                Console.WriteLine("User was not registered!");
            }

            return user;
        }

        public void PrintUser(User user)
        {
            _trainSystemContext.Entry(user).Reference(u => u.Card).Load();
            TableFormatPrinter.PrintRow("Id", "Name", "Age", "Card");
            TableFormatPrinter.PrintLine();
            string cardtype = CardType.GetCardType(user.Card);
            TableFormatPrinter.PrintRow($"{user.Id}", $"{user.Name}", $"{user.Age}", $"{cardtype}");
            TableFormatPrinter.PrintLine();
        }

        public void PrintAllUsers(int adminId)
        {
            var admin = SearchUserById(adminId);
            if (admin == null)
            {
                Console.WriteLine("Not valid id.");
                return;
            }

            if (admin.IsAdmin)
            {
                var users = _trainSystemContext.Users
                    .Include(u => u.Card)
                    .ToList();
                TableFormatPrinter.PrintLine();
                TableFormatPrinter.PrintRow("Id", "Name", "Age", "Card");
                TableFormatPrinter.PrintLine();
                foreach (var user in users)
                {
                    string cardtype = CardType.GetCardType(user.Card);
                    TableFormatPrinter.PrintRow($"{user.Id}", $"{user.Name}", $"{user.Age}", $"{cardtype}");
                    TableFormatPrinter.PrintLine();
                }
            }
            else
            {
                Console.WriteLine("Not an admin.");
            }


        }

        public User? EditUser(int adminId, int userId, string? newName, int? newAge, DiscountCard? newCard)
        {
            var admin = _trainSystemContext.Users.FirstOrDefault(u => u.Id == adminId && u.IsAdmin);

            if (admin == null)
            {
                Console.WriteLine("No admin rights");
                return null;
            }

            var userToUpdate = _trainSystemContext.Users.FirstOrDefault(u => u.Id == userId);

            if (userToUpdate == null)
            {
                Console.WriteLine("No such user!");
                return null;
            }

            if (newName != null)
            {
                userToUpdate.Name = newName;
                Console.WriteLine("name");
            }

            if (newAge != null && newAge > 0)
            {
                userToUpdate.Age = (int)newAge;
                Console.WriteLine("age");

            }

            if (newCard != null)
            {
                if (userToUpdate.Card != null)
                {
                    Console.WriteLine("User already have a card!");
                }
                else
                {
                    newCard.UserId = userToUpdate.Id;
                    userToUpdate.Card = newCard;
                    Console.WriteLine("card");
                }
            }

            _trainSystemContext.SaveChanges();
            Console.WriteLine("User was updated!");
            return userToUpdate;
        }

        public void PrintAllUserTickets(User user)
        {

            var tickets = _trainSystemContext.Ticket
               .Include(t => t.Train)
               .Include(t => t.User)
               .AsEnumerable()
               .Where(b => b.UserId == user.Id);

            if (tickets == null)
            {
                Console.WriteLine("User have no Tickets.");
                return;
            }

            TableFormatPrinter.PrintLine();
            TableFormatPrinter.PrintRow("Id", "Price", "Start", "Destination", "Date");
            TableFormatPrinter.PrintLine();
            foreach (var ticket in tickets)
            {
                TableFormatPrinter.PrintRow($"{ticket.Id}", $"{ticket.Price}",
                    $"{ticket.Train.DepartureStation}", $"{ticket.Train.DestinationStation}",
                    $"{ticket.DepartureDate}");
                TableFormatPrinter.PrintLine();
            }
        }

        public void PrintAllActiveUserTickets(User user)
        {
            var bookings = _trainSystemContext.Bookings
               .Include(b => b.Ticket)
               .ThenInclude(t => t.Train)
               .Include(b => b.User)
               .AsEnumerable()
               .Where(b => b.UserId == user.Id && b.Active);

            if (bookings == null)
            {
                Console.WriteLine("User have no booked Tickets.");
                return;
            }

            TableFormatPrinter.PrintLine();
            TableFormatPrinter.PrintRow("BId", "Price", "Start", "Destination", "Date", "Active");
            TableFormatPrinter.PrintLine();
            foreach (var booking in bookings)
            {
                TableFormatPrinter.PrintRow($"{booking.Id}", $"{booking.Ticket.Price}",
                    $"{booking.Ticket.Train.DepartureStation}", $"{booking.Ticket.Train.DestinationStation}",
                    $"{booking.Ticket.DepartureDate}", $"{booking.Active}");
                TableFormatPrinter.PrintLine();
            }
        }

        public void PrintAllCanceledUserTickets(User user)
        {
            var bookings = _trainSystemContext.Bookings
               .Include(b => b.Ticket)
               .ThenInclude(t => t.Train)
               .Include(b => b.User)
               .AsEnumerable()
               .Where(b => b.UserId == user.Id && !b.Active);

            if (bookings == null)
            {
                Console.WriteLine("User have no booked Tickets.");
                return;
            }

            TableFormatPrinter.PrintLine();
            TableFormatPrinter.PrintRow("BId", "Price", "Start", "Destination", "Date", "Active");
            TableFormatPrinter.PrintLine();
            foreach (var booking in bookings)
            {
                TableFormatPrinter.PrintRow($"{booking.Id}", $"{booking.Ticket.Price}",
                    $"{booking.Ticket.Train.DepartureStation}", $"{booking.Ticket.Train.DestinationStation}",
                    $"{booking.Ticket.DepartureDate}", $"{booking.Active}");
                TableFormatPrinter.PrintLine();
            }
        }

        public void ApplyAdminDiscount(int adminId, int ticketId)
        {
            var admin = _trainSystemContext.Users.FirstOrDefault(u => u.Id == adminId && u.IsAdmin);

            if (admin == null)
            {
                Console.WriteLine("No admin rights");
                return;
            }

            var booking = _trainSystemContext.Bookings
               .Include(b => b.Ticket)
               .ThenInclude(t => t.Train)
               .Include(b => b.User)
               .AsEnumerable()
               .FirstOrDefault(b => b.Ticket.Id == ticketId);

            if (booking == null)
            {
                Console.WriteLine($"User have no Ticket with id {ticketId}.");
                return;
            }

            booking.Ticket.Price = _ticketService.CalculateTicketPrice(
                booking.Ticket.Price,
                1,
                booking.Ticket.Train.DepartureDate,
                true,
                booking.User.Card
                );

        }

        public void ApplyAdminDiscount(int adminId, int userId, Ticket ticket)
        {
            var admin = _trainSystemContext.Users.FirstOrDefault(u => u.Id == adminId && u.IsAdmin);

            if (admin == null)
            {
                Console.WriteLine("No admin rights");
                return;
            }

            var user = _trainSystemContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                Console.WriteLine("No such user!");
                return;
            }

            ticket.Price = _ticketService.CalculateTicketPrice(ticket.Price, 1, ticket.DepartureDate, false, user.Card);

            user.Tickets.Add(ticket);
            ticket.User = user;
            _trainSystemContext.SaveChanges();
        }

        private static string GenerateHashedPassword(string password)
        {
            // Generate a random salt for the password
            string salt = GenerateSalt();

            // Hash the password with the salt
            string hashedPassword = HashPassword(password, salt);
            return String.Concat(hashedPassword, delimiter, salt);
        }

        private static string GenerateSalt()
        {
            // Generate a random salt using the RandomNumberGenerator
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        private static string HashPassword(string password, string salt)
        {
            // Concatenate the password and salt
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = System.Text.Encoding.UTF8.GetBytes(salt);

            byte[] saltedPasswordBytes = new byte[passwordBytes.Length + salt.Length];
            Array.Copy(passwordBytes, saltedPasswordBytes, passwordBytes.Length);
            Array.Copy(saltBytes, 0, saltedPasswordBytes, passwordBytes.Length, salt.Length);

            // Hash the salted password using the SHA256 algorithm
            byte[] hashedPassword;
            using (var sha256 = SHA256.Create())
            {
                hashedPassword = sha256.ComputeHash(saltedPasswordBytes);
            }
            return Convert.ToBase64String(hashedPassword);
        }

    }
}
