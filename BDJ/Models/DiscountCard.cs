using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Models
{
    public class DiscountCard
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }

        //[ForeignKey()]
        public int UserId { get; set; }
    }
}
