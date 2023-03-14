﻿using BDJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Services
{
    public class BookingService
    {
        private readonly TrainSystemContext _trainSystemContext;
        private TicketService _ticketService;
        private TrainService _trainService;
        private UserService _userService;
        public BookingService(TrainSystemContext trainSystemContext)
        {
            _trainSystemContext = trainSystemContext;
            _ticketService = new TicketService(_trainSystemContext);
            _trainService = new TrainService(_trainSystemContext);
            _userService = new UserService(_trainSystemContext);
        }


        public void bookTicket(User user, string departureStation, string destination, double price, DateTime date)
        {

            var trains = _trainService.searchTrainByDateAndDestination(destination, date);

            Console.WriteLine(trains.ToList().Count);



            if (trains == null)
            {
                Console.WriteLine($"No Trains found going to {destination} on {date}");
                return;
            }

            var train = trains.First();

            Console.WriteLine(train.ToString());

            var ticket = _ticketService.createTicket(train, price, date);
            Console.WriteLine(ticket.ToString());

            var booking = new Booking { Ticket = ticket, TicketId = ticket.Id, User = user, UserId = user.Id, active = true };
            Console.WriteLine(booking.ToString());

            user.Tickets.Add(ticket);
            user.Bookings.Add(booking);

            _trainSystemContext.Bookings.Add(booking);
            _trainSystemContext.SaveChanges();
        }
    }
}
