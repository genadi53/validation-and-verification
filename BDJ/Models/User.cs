using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsAdmin { get; set; }
        public DiscountCard? Card { get; set; }
        public List<Ticket> Tickets { get; } = new();
        public List<Booking> Bookings { get; } = new();

    }
}
