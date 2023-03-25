using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public bool IsAdmin { get; set; }
        public virtual DiscountCard? Card { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }

        public User()
        {
           Bookings = new HashSet<Booking>();
        }
    }
}
