using BDJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Services
{
    public class UserService
    {
        private readonly TrainSystemContext _trainSystemContext;
        private const string delimiter = "#?";

        public UserService(TrainSystemContext trainSystemContext)
        {
            _trainSystemContext = trainSystemContext;
        }


        public User? AddUser(string name, int age, string password, bool isAdmin, DiscountCard? card)
        {
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

        public User? Login(string name, string password)
        {
            var user = _trainSystemContext.Users.FirstOrDefault(u => u.Name == name);

            if (user == null)
            {
                Console.WriteLine("User was not found");
                return null;
            }

            string? savedSalt = user.Password.Split(delimiter).Last();


            Console.WriteLine($"saved pass : {user.Password}");
            Console.WriteLine($"savedSalt : {savedSalt}");


            var hashedPassword = HashPassword(password, savedSalt);
            Console.WriteLine(hashedPassword);

            if (user.Password.Equals(String.Concat(hashedPassword, delimiter, savedSalt)))
            {
                Console.WriteLine("Welcome!");
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

        public void PrintUsers(int adminId)
        {
            var admin = SearchUserById(adminId);
            if (admin == null)
            {
                Console.WriteLine("Not valid id.");
                return;
            }

            if (admin.IsAdmin)
            {
                var users = _trainSystemContext.Users.ToList();
                foreach (var user in users)
                {
                    Console.WriteLine($"{user.Id} - {user.Name} - {user.Age}");
                }
            }
            else
            {
                Console.WriteLine("Not an admin.");
                return;
            }


        }

        private static string GenerateHashedPassword(string password)
        {
            // Generate a random salt for the password
            string salt = GenerateSalt();

            // Hash the password with the salt
            string hashedPassword = HashPassword(password, salt);

            Console.WriteLine("Salt: {0}", salt);
            Console.WriteLine("Hashed Password: {0}", hashedPassword);

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
