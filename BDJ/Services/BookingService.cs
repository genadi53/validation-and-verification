using BDJ.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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


        public void BookTicket(User user, string departureStation, string destination, double price, DateTime date, bool withChild)
        {

            var trains = _trainService.SearchTrainByDateAndStations(departureStation, destination, date).ToList();

            if (trains == null || trains.Count <= 0)
            {
                Console.WriteLine($"No Trains found going from {departureStation} to {destination} on {date}");
                return;
            }

            var train = trains.First();
            int numTickets = withChild ? 2 : 1;
            var priceWithDiscount = _ticketService.CalculateTicketPrice(price, numTickets, date, withChild, user.Card);
            
            if(priceWithDiscount <= 0)
            {
                Console.WriteLine($"Invalid provided price!");
                return;
            }

            var ticket = _ticketService.AddTicket(user, train, priceWithDiscount, train.DepartureDate);
            var booking = new Booking { Ticket = ticket, TicketId = ticket.Id, User = user, UserId = user.Id, Active = true };

            //_trainSystemContext.Users.First(u => u.Id == user.Id).Bookings.Add(booking);
            user.Bookings.Add(booking);
            _trainSystemContext.Bookings.Add(booking);
            _trainSystemContext.SaveChanges();
            Console.WriteLine("Booking completed");
        }

        public void PrintAllBookings()
        {
            //_trainSystemContext.ChangeTracker.Clear();
            var bookings = _trainSystemContext.Bookings
               .Include(b => b.Ticket)
               .ThenInclude(t => t.Train)
               .Include(b => b.User)
               .ToList();

            TableFormatPrinter.PrintLine();
            TableFormatPrinter.PrintRow("BId", "User", "Price", "Start", "Destination", "Date", "Active");
            TableFormatPrinter.PrintLine();
            foreach (var booking in bookings)
            {
                TableFormatPrinter.PrintRow($"{booking.Id}",
                   $"{booking.User.Name}", $"{booking.Ticket.Price}",
                    $"{booking.Ticket.Train.DepartureStation}", $"{booking.Ticket.Train.DestinationStation}",
                    $"{booking.Ticket.DepartureDate}", $"{booking.Active}");
                TableFormatPrinter.PrintLine();
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

            _trainSystemContext.Entry(foundUser).Collection(u => u.Bookings).Load();

            var booking = _trainSystemContext.Bookings
           .Include(b => b.Ticket)
           .ThenInclude(t => t.Train)
           .Include(b => b.User)
           .AsEnumerable()
           .FirstOrDefault((booking) =>
            {
                //_trainSystemContext.Entry(booking).Reference(booking => booking.Ticket).Load();
                //_trainSystemContext.Entry(booking).Reference(booking => booking.User).Load();

                if (booking.Ticket.Train.DestinationStation.ToLower().Equals(destination.ToLower())
                    && booking.Ticket.Train.DepartureStation.ToLower().Equals(departureStation.ToLower())
                    //&& Time.EqualsUpToSeconds(booking.Ticket.DepartureDate, date)
                    && Time.SameDay(booking.Ticket.DepartureDate, date)
                )
                {
                    return true;
                }
                return false;
            });

            if (booking == null)
            {
                Console.WriteLine("No Bookings Found!");
                return;
            }

            if (DateTime.Compare(booking.Ticket.DepartureDate, DateTime.Now) >= 0)
            {
                booking.Active = false;
                _trainSystemContext.SaveChanges();
                Console.WriteLine("Booking is Cancelled.");
            }
            else
            {
                Console.WriteLine("Booking Date is passed Cancellation.");
            }

        }

        public void UpdateBookingDate(User user, string departureStation, string destination, DateTime oldDate, DateTime newDate)
        {
            var foundUser = _userService.SearchUserById(user.Id);

            if (foundUser == null)
            {
                Console.WriteLine("No Such User!");
                return;
            }

            _trainSystemContext.Entry(foundUser).Collection(u => u.Bookings).Load();

            var booking = _trainSystemContext.Bookings
                .Include(b => b.Ticket)
                .ThenInclude(ticket => ticket.Train)
                .Include(b => b.User)
                .AsEnumerable()
                .FirstOrDefault((booking) =>
                {
                    if (booking.UserId != foundUser.Id)
                    {
                        return false;
                    }

                    if (booking.Ticket.Train.DestinationStation.ToLower().Equals(destination.ToLower())
                        && booking.Ticket.Train.DepartureStation.ToLower().Equals(departureStation.ToLower())
                         //&& Time.EqualsUpToSeconds(booking.Ticket.DepartureDate, date)
                         && Time.SameDay(booking.Ticket.DepartureDate, oldDate)
                    )
                    {
                        return true;
                    }
                    return false;
                });

            if (booking == null)
            {
                Console.WriteLine($"No Bookings Found from {departureStation} to {destination} on {oldDate}!");
                return;
            }


            if (DateTime.Compare(booking.Ticket.DepartureDate, DateTime.Now) >= 0)
            {
                _trainSystemContext.Entry(booking).Reference(booking => booking.Ticket).Load();
                booking.Ticket.DepartureDate = newDate;

                _trainSystemContext.Bookings.First(b => b.Id == booking.Id).Ticket.DepartureDate = newDate;
                _trainSystemContext.Bookings.Update(booking);

                var ticket = _trainSystemContext.Ticket.First(b => b.Id == booking.TicketId);
                ticket.DepartureDate = newDate;
                _trainSystemContext.Ticket.Update(ticket);

                _trainSystemContext.SaveChanges();
                Console.WriteLine("Updated");
            }
            else
            {
                Console.WriteLine("Booking Date is passed and it can't be updated.");
            }
        }
    }
}
