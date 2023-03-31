using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BDJ
{
    public static class TableFormatPrinter
    {
        static readonly int tableWidth = 110;

        public static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        public static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            StringBuilder row = new StringBuilder();
            row.Append('|');

            foreach (string column in columns)
            {
                row.Append(AlignCentre(column, width) + "|");
            }

            Console.WriteLine(row.ToString());
        }

        public static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? String.Concat(text.AsSpan(0, width-3), "...") : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}
