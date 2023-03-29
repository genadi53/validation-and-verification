using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static bool SameDay(DateTime date1, DateTime date2)
        {
            return date1.DayOfYear.Equals(date2.DayOfYear);
        }

        public static bool EqualsUpToSeconds(DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day &&
                   date1.Hour == date2.Hour && date1.Minute == date2.Minute && date1.Second == date2.Second;
        }

        public static DateTime GetDateInput()
        {
            while (true)
            {
                Console.WriteLine("Enter date: ");
                DateTime date;
                //DateTime.TryParseExact(Console.ReadLine(), "dd'.'MM'.'yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out date)
                if (DateTime.TryParse(Console.ReadLine(), CultureInfo.CreateSpecificCulture("en-GB"), DateTimeStyles.None, out date))
                {
                    Console.WriteLine(date.ToString());
                    return date;
                }
                else
                {
                    Console.WriteLine("You have entered an incorrect date value.");
                }
            }
        }
    }
}
