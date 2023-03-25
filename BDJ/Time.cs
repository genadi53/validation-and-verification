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

        public static bool EqualsUpToSeconds(DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day &&
                   date1.Hour == date2.Hour && date1.Minute == date2.Minute && date1.Second == date2.Second;
        }
    }
}
