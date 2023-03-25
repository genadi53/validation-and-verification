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

//userService.Register("test", "test", 21);
//userService.Login("test", "test");

//foreach (var t in db.Trains.ToList())
//{
//    Console.WriteLine($"{t.Id} - from {t.DepartureStation} to {t.DestinationStation} on {t.DepartureDate}");
//}


//foreach (var t in db.Users.ToList())
//{
//    Console.WriteLine($"{t.Id} - {t.Name} - {t.Age}");
//}

var user = db.Users.First();
//Console.WriteLine($"{user.Id} - {user.Name}");
// Plovdiv to Pernik on 3/23/2023 2:00:00 AM
//bookingService.BookTicket(user, "Plovdiv", "Pernik", 100, new DateTime(2023, 3, 23, 2, 0, 0));

Console.WriteLine("\n*********************************");
foreach (var booking in db.Bookings)
{
    db.Entry(booking).Reference(booking => booking.Ticket).Load();
    db.Entry(booking.Ticket).Reference(t => t.Train).Load();
    //_trainSystemContext.Entry(booking).Reload();

    Console.WriteLine($"{booking.Id} was booked by {booking.User.Name}, " +
        $"ticket N{booking.TicketId} " +
        $"from {booking.Ticket.Train.DepartureStation} " +
        $"to {booking.Ticket.Train.DestinationStation} " +
        $"on {booking.Ticket.Train.DepartureDate} " +
        $"is {booking.active}");
}
Console.WriteLine("*********************************\n");


foreach (var booking in db.Bookings)
{
    db.Entry(booking).Reference(booking => booking.Ticket).Load();
    db.Entry(booking.Ticket).Reference(t => t.Train).Load();

    Console.WriteLine($"{booking.Id} - {booking.Ticket.Train.DepartureStation} - {booking.Ticket.Train.DestinationStation} " +
         $"- {booking.Ticket.DepartureDate} - {booking.active}");
}

//bookingService.PrintAllBookings();
bookingService.CancelBooking(user, "Plovdiv", "Pernik", new DateTime(2023, 3, 23, 22, 46, 17));
//bookingService.UpdateBookingDate(user!, "Plovdiv", "Pernik", new DateTime(2023, 3, 22, 2, 0, 0), DateTime.Now);
//bookingService.PrintAllBookings();

Console.WriteLine("---- User's bookings -----");
foreach(var booking in user.Bookings)
{
    Console.WriteLine($"{booking.Id} - {booking.Ticket.Train.DepartureStation} - {booking.Ticket.Train.DestinationStation} " +
         $"- {booking.Ticket.DepartureDate} - {booking.active}");
}

bookingService.PrintAllBookings();

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