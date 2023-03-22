using BDJ.Models;
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
        private readonly TicketService _ticketService;
        private readonly TrainService _trainService;
        private readonly UserService _userService;
        public BookingService(TrainSystemContext trainSystemContext)
        {
            _trainSystemContext = trainSystemContext;
            _ticketService = new TicketService(_trainSystemContext);
            _trainService = new TrainService(_trainSystemContext);
            _userService = new UserService(_trainSystemContext);
        }


        public void BookTicket(User user, string departureStation, string destination, double price, DateTime date) // bool withChild
        {

            var trains = _trainService.SearchTrainByDateAndDestination(destination, date);

            Console.WriteLine(trains.ToList().Count);



            if (trains == null)
            {
                Console.WriteLine($"No Trains found going to {destination} on {date}");
                return;
            }

            var train = trains.First();

            Console.WriteLine(train.ToString());

            //var priceWithDiscount = _ticketService.CalculateTicketPrice(price, date, withChild, user.Card);

            var ticket = _ticketService.AddTicket(train, price, date);
            Console.WriteLine(ticket.ToString());

            var booking = new Booking { Ticket = ticket, TicketId = ticket.Id, User = user, UserId = user.Id, active = true };
            Console.WriteLine(booking.ToString());

            user.Tickets.Add(ticket);
            user.Bookings.Add(booking);

            _trainSystemContext.Bookings.Add(booking);
            _trainSystemContext.SaveChanges();
        }

        public void PrintAllBookings()
        {
            foreach (var booking in _trainSystemContext.Bookings.ToList())
            {
                _trainSystemContext.Entry(booking).Reference(b => b.Ticket).Load();
                _trainSystemContext.Entry(booking).Reference(b => b.User).Load();

                Console.WriteLine($"Booking with id {booking.Id} was booked by {booking.User.Name}, who travels with ticket N{booking.TicketId} going to {booking.Ticket.Train.DestinationStation} " +
                    $"from {booking.Ticket.Train.DepartureStation} on {booking.Ticket.Train.DepartureDate} is {booking.active}");
            }
        }


        public void CancelBooking(User user, string departureStation, string destination, DateTime date)
        {
            var foundUser = _userService.SearchUserById(user.Id);

            if (foundUser == null)
            {
                Console.WriteLine("No Such User!");
                return;
            }

            //var trains = _trainService.SearchTrainByDateAndDestination(destination, date);
            //if (trains == null || trains.ToList().Count == 0)
            //{
            //    Console.WriteLine("Trains Not Found!");
            //    return;
            //}

            _trainSystemContext.Entry(user).Collection(u => u.Bookings).Load();
            var booking = user.Bookings.ToList().First((booking) =>
            {
                _trainSystemContext.Entry(booking).Reference(booking => booking.Ticket).Load();
                _trainSystemContext.Entry(booking).Reference(booking => booking.User).Load();
                //Console.WriteLine(booking.Id);
                //Console.WriteLine(booking.Ticket.Train.DepartureStation);
                //Console.WriteLine(booking.Ticket.Train.DestinationStation);
                //Console.WriteLine(booking.Ticket.DepartureDate);

                if (booking.Ticket.Train.DestinationStation == destination && booking.Ticket.Train.DepartureStation == departureStation && booking.Ticket.DepartureDate == date)
                {
                    return true;
                }
                return false;
            });

            if(booking == null)
            {
                Console.WriteLine("No Bookings Found!");
                    return;
            }

            Console.WriteLine(booking.Id);
            Console.WriteLine(booking.Ticket.Train.DepartureStation);
            Console.WriteLine(booking.Ticket.Train.DestinationStation);
            Console.WriteLine(booking.Ticket.DepartureDate);

           
            if(booking.Ticket.DepartureDate >= DateTime.Now)
            {
                booking.active = false;
                _trainSystemContext.SaveChanges();
            } else
            {
                Console.WriteLine("Booking Date is passed Cancellation.");
            }

        }

        //change booking date if its still active

    }
}
