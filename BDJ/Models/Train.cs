using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Models
{
    public class Train
    {
        public int Id { get; set; }
        public string DepartureStation { get; set; }
        public string DestinationStation { get; set; }
        public int Seats { get; set; }
    }
}
