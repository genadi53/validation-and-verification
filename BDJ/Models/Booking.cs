using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public DateTime DepartureDate { get; set; }
        public double Price { get; set; }

    }
}
