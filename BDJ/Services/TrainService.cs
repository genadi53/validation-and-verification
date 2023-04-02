using BDJ.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Services
{
    public class TrainService
    {

        private readonly TrainSystemContext _trainSystemContext;
        private readonly List<string> cityNames = new List<string> {
                "Sofia", "Plovdiv", "Varna", "Burgas",
                "Stara Zagora", "Ruse", "Pleven", "Sliven",
                "Pernik", "Dobrich", "Sliven",
                "Veliko Tarnovo", "Vratsa", "Blagoevgrad", "Haskovo"
        };
        private readonly List<Tuple<string, string>> cityPairs = new List<Tuple<string, string>>();


        public TrainService(TrainSystemContext context)
        {
            _trainSystemContext = context;
        }

        public IQueryable<Train> SearchTrainByDestination(string destination)
        {
            return _trainSystemContext.Trains.OrderBy(train => train.DepartureDate)
                .Where(train => train.DestinationStation.ToLower().Equals(destination.ToLower())
                  //&& train.DepartureDate.DayOfYear >= DateTime.Now.DayOfYear
                );
        }

        public IQueryable<Train> SearchTrainByDepartureDate(DateTime date)
        {
            return _trainSystemContext.Trains.Where(train => train.DepartureDate.DayOfYear == date.DayOfYear);
        }

        public IQueryable<Train> SearchTrainByDateAndDestination(string destination, DateTime date)
        {

            IQueryable<Train> trains = _trainSystemContext.Trains.Where(train => train.DestinationStation.ToLower().Equals(destination.ToLower())
            && train.DepartureDate.DayOfYear == date.DayOfYear);
            return trains;
        }

         public IQueryable<Train> SearchTrainByDateAndStations(string departureStation, string destination, DateTime date)
        {

            IQueryable<Train> trains = _trainSystemContext.Trains.Where(train => train.DestinationStation.ToLower().Equals(destination.ToLower())
            && train.DepartureStation.ToLower().Equals(departureStation.ToLower())
            && train.DepartureDate.DayOfYear == date.DayOfYear);
            return trains;
        }

        public void AddTrain(int seats, string staringStation, string destiinationStation, DateTime date)
        {
            var train = new Train { DepartureStation = staringStation, DestinationStation = destiinationStation, DepartureDate = date, Seats = seats };
            _trainSystemContext.Trains.Add(train);
            _trainSystemContext.SaveChanges();
        }

        private void GenerateCityPairs()
        {
            var random = new Random();
            for (int i = 0; i < cityNames.Count; i++)
            {
                string city1 = cityNames[i];
                string city2;

                do
                {
                    int randomIndex = random.Next(cityNames.Count);
                    city2 = cityNames[randomIndex];
                } while (city1 == city2);

                cityPairs.Add(new Tuple<string, string>(city1, city2));
            }
        }

        public void MockDailyTrains()
        {

            GenerateCityPairs();

            var random = new Random();
            foreach (var pair in cityPairs)
            {
                Console.WriteLine($"{pair.Item1} - {pair.Item2}");
                var train = new Train { DepartureStation = pair.Item1, DestinationStation = pair.Item2, DepartureDate = DateTime.Today.AddHours(random.Next(24)), Seats = 100 };
                _trainSystemContext.Trains.Add(train);
                _trainSystemContext.SaveChanges();
            }

        }

        public void PrintDailyTrains()
        {
            var trains = _trainSystemContext.Trains.Where(train => train.DepartureDate.DayOfYear == DateTime.Now.DayOfYear).AsQueryable();
            PrintGivenTrains(trains);
        }

        public static void PrintGivenTrains(IQueryable<Train>? trains)
        {
            if (trains == null || !trains.Any())
            {
                Console.WriteLine("No trains found!");
                return;
            }
            TableFormatPrinter.PrintLine();
            TableFormatPrinter.PrintRow("Id", "Start", "Destination", "Date");
            TableFormatPrinter.PrintLine();
            foreach (var train in trains)
            {
                TableFormatPrinter.PrintRow($"{train.Id}", $"{train.DepartureStation}", $"{train.DestinationStation}", $"{train.DepartureDate}");
                TableFormatPrinter.PrintLine();
            }
        }
    }
}