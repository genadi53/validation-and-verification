﻿using BDJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Services
{
    public class TrainService
    {
        private readonly TrainSystemContext _trainSystemContext = new TrainSystemContext();
        List<string> cityNames = new List<string> {
                "Sofia", "Plovdiv", "Varna", "Burgas",
                "Stara Zagora", "Ruse", "Pleven", "Sliven",
                "Pernik", "Dobrich", "Sliven",
                "Veliko Tarnovo", "Vratsa", "Blagoevgrad", "Haskovo"
        };
        List<Tuple<string, string>> cityPairs = new List<Tuple<string, string>>();


        public TrainService(TrainSystemContext context)
        {
            _trainSystemContext = context;
        }

        public Train? searchTrainByDestination(string destination)
        {
            var train = _trainSystemContext.Trains.OrderBy(train => train.DepartureDate)
                .First(train => train.DestinationStation.Equals(destination));
            return train;
        }

        public Train? searchTrainByDepartureDate(DateTime date)
        {
            var train = _trainSystemContext.Trains.First(train => train.DestinationStation.Equals(date));
            return train;
        }

        public void createTrain(int seats, string staringStation, string destiinationStation, DateTime date)
        {
            var train = new Train { DepartureStation = staringStation, DestinationStation = destiinationStation, DepartureDate = date, Seats = seats };
            _trainSystemContext.Trains.Add(train);
            _trainSystemContext.SaveChanges();
        }

        private void generateCityPairs()
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

        public void mockDailyTrains()
        {

            generateCityPairs();

            var random = new Random();
            foreach (var pair in cityPairs)
            {
                Console.WriteLine($"{pair.Item1} - {pair.Item2}");
                //createTrain(100, destinations[i], destinations[(i + 1)], );
                var train = new Train { DepartureStation = pair.Item1, DestinationStation = pair.Item2, DepartureDate = DateTime.Today.AddHours(random.Next(24)), Seats = 100 };
                _trainSystemContext.Trains.Add(train);
                _trainSystemContext.SaveChanges();
            }

        }
    }

}