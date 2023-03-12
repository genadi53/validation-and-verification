using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime DepartureDate { get; set; }
        public double Price { get; set; }
        public int TrainId { get; set; }
        public Train? Train { get; set; }
    }
}
