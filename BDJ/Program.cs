using BDJ;
using BDJ.Models;
using BDJ.Services;
using System;
using System.Linq;

using var db = new TrainSystemContext();


//Menu.DisplayMenu();

Console.WriteLine(db.DbPath);

BookingService bookingService = new BookingService(db);
UserService userService = new UserService(db);
TrainService trainService = new TrainService(db);
//trainService.MockDailyTrains();

//userService.Register("test12343", "test12343", 21);
userService.Login("test12343", "test12343");

//foreach (var t in db.Trains.ToList())
//{
//    Console.WriteLine($"{t.Id} - from {t.DepartureStation} to {t.DestinationStation} on {t.DepartureDate}");
//}

// "3/14/2023 11:00:00 PM"
// Haskovo to Burgas on 3/22/2023 2:00:00 AM
//bookingService.BookTicket(db.Users.First(), "Haskovo", "Pernik", 100, new DateTime(2023, 3, 14, 23, 0, 0));

//foreach (var t in db.Users.ToList())
//{
//    Console.WriteLine($"{t.Id} - {t.Name} - {t.Age}");
//}

//var user = db.Users.First(u => u.Bookings.ToList().Count > 0);
//Console.WriteLine($"{user.Id} - {user.Name}");
//bookingService.PrintAllBookings();
////bookingService.CancelBooking(user, "Haskovo", "Pernik", new DateTime(2023, 3, 14, 23, 0, 0));
//bookingService.UpdateBookingDate(user, "Haskovo", "Pernik", new DateTime(2023, 3, 22, 20, 48, 34), DateTime.Now);
//bookingService.PrintAllBookings();

//// Update
//Console.WriteLine("Updating the blog and adding a post");
//blog.Url = "https://devblogs.microsoft.com/dotnet";
//blog.Posts.Add(
//    new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
//db.SaveChanges();

//// Delete
//Console.WriteLine("Delete the blog");
//db.Remove(blog);
//db.SaveChanges();