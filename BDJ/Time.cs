using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ
{
    public class Time
    {
        public static bool isTimeBetween(TimeSpan start, TimeSpan end, TimeSpan timeToCheck)
        {
            return timeToCheck >= start && timeToCheck <= end;

        }
    }
}
