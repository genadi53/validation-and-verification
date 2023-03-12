using BDJ;
using BDJ.Models;
using System;
using System.Linq;

using var db = new TrainSystemContext();

// Note: This sample requires the database to be created before running.
Console.WriteLine($"Database path: {db.DbPath}.");
// Create
//Console.WriteLine("Inserting a new blog");
//db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
//db.SaveChanges();

//db.Add(new User { Age = 21, IsAdmin = false, Name = "Ivan Ivan" });
//db.SaveChanges();

// Read
//Console.WriteLine("Querying for a blog");
//var blog = db.Blogs
//    .OrderBy(b => b.BlogId)
//    .First();

Console.WriteLine(db.Users.First().Name);

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