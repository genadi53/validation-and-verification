using BDJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ
{
    internal static class CardType
    {
        public static string GetCardType(DiscountCard? card)
        {
            if(card == null)
            {
                return "no card";
            }

            if (card.Type == "family") return "family";
            if (card.Type == "senior") return "senior";
            return "not valid card";
        }

    }
}
