﻿using BDJ;
using BDJ.Models;
using BDJ.Services;
using System;
using System.Linq;

using var db = new TrainSystemContext();


Console.WriteLine(db.DbPath);

BookingService bookingService = new BookingService(db);
UserService userService = new UserService(db);
TrainService trainService = new TrainService(db);
//trainService.mockDailyTrains();

foreach (var t in db.Trains.ToList())
{
    Console.WriteLine($"{t.Id} - from {t.DepartureStation} to {t.DestinationStation} on {t.DepartureDate}");
}

// "3/14/2023 11:00:00 PM"
bookingService.bookTicket(db.Users.First(), "Haskovo", "Pernik", 100, new DateTime(2023, 3, 14, 23 ,0 ,0));

//foreach (var t in db.Users.ToList())
//{
//    Console.WriteLine($"{t.Id} - {t.Name} - {t.Age}");
//}


var booking  = db.Bookings.Last();
Console.WriteLine($"{booking.Id} - {booking.User.Name} travels with ticket N{booking.TicketId} {booking.active} {booking.Ticket.Price}");

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