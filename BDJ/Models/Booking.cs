using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDJ.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Ticket))]
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public bool Active { get; set; }
    }
}
