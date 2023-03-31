using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDJ.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BDJ
{

    public class TrainSystemContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<DiscountCard> DiscountCards { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public string DbPath { get; }

        public TrainSystemContext() : base()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "train_system.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
