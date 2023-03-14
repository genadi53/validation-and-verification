using BDJ;
using BDJ.Models;
using BDJ.Services;
using System;
using System.Linq;

using var db = new TrainSystemContext();


Console.WriteLine(db.DbPath);

TrainService trainService = new TrainService(db);
//trainService.mockDailyTrains();

foreach (var t in db.Trains.ToList())
{
    Console.WriteLine($"{t.Id} - from {t.DepartureStation} to {t.DestinationStation} on {t.DepartureDate}");
}

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