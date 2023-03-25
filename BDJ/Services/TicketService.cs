using BDJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Services
{
    public class TicketService
    {
        private readonly TrainSystemContext _trainSystemContext;
        private readonly TimeSpan earlyTraficStart = new TimeSpan(7, 30, 0);
        private readonly TimeSpan earlyTraficEnd = new TimeSpan(9, 30, 0);
        private readonly TimeSpan lateTraficStart = new TimeSpan(16, 0, 0);
        private readonly TimeSpan lateTraficEnd = new TimeSpan(19, 30, 0);

        public TicketService(TrainSystemContext trainSystemContext)
        {
            _trainSystemContext = trainSystemContext;
        }

        public double CalculateTicketPrice(double tickePrice, int numberOfTickets, DateTime trainLeaveTime, bool withChild, DiscountCard? card)
        {
            if (tickePrice <= 0 || numberOfTickets <= 0) return -1;

            double discount = 1;
            if (withChild)
            {
                if ((card != null) && (card.Type == "family"))
                {
                    //discount = discount > 0.5 ? 0.5 : discount;
                    discount = 0.5;
                }
                else
                {
                    //discount = discount > 0.9 ? 0.9 : discount;
                    discount = 0.9;
                }

            }

            if ((card != null) && (card.Type == "senior"))
            {
                //discount = discount > 0.66 ? 0.66 : discount;
                discount = 0.66;
            }

            bool isInEarlyTraffic = Time.isTimeBetween(earlyTraficStart, earlyTraficEnd, trainLeaveTime.TimeOfDay);
            bool isInLateTraffic = Time.isTimeBetween(lateTraficStart, lateTraficEnd, trainLeaveTime.TimeOfDay);
            if (isInEarlyTraffic || isInLateTraffic)
            {
                discount = discount >= 1 ? 1 : discount;
            }
            else
            {
                discount = discount >= 0.95 ? 0.95 : discount;
            }

            return numberOfTickets * tickePrice * discount;

        }

        public double CalculateTwoWayTicketPrice(bool isTwoWay, double tickePrice, int numberOfTickets, DateTime trainLeaveTime, bool withChild, DiscountCard? card)
        {
            double price = CalculateTicketPrice(tickePrice, numberOfTickets, trainLeaveTime, withChild, card);

            if(price < 0)
            {
                return -1;
            }

            return isTwoWay ? 2 * price : price;
        }

        public Ticket AddTicket(User user, Train train, double price, DateTime date)
        {
            Ticket ticket = new Ticket { DepartureDate = date, Price = price, Train = train, TrainId=train.Id, UserId=user.Id, User=user };
            _trainSystemContext.Ticket.Add(ticket);
            _trainSystemContext.Users.First(u => u.Id == user.Id).Tickets.Add(ticket);
            _trainSystemContext.SaveChanges();
            return ticket;
        }
    }
}
