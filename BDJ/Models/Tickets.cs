using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public DateTime DepartureDate { get; set; }
        public double Price { get; set; }

        [ForeignKey(nameof(Train))]
        public int TrainId { get; set; }
        public Train Train { get; set; }
    }
}
