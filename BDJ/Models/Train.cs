using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Models
{
    public class Train
    {
        [Key]
        public int Id { get; set; }
        public string DepartureStation { get; set; }
        public string DestinationStation { get; set; }
        public DateTime DepartureDate { get; set; }
        public int Seats { get; set; }
        public ICollection<Ticket>? tickets { get; set; }

        public Train()
        {
            tickets = new HashSet<Ticket>();
        }

    }
}
