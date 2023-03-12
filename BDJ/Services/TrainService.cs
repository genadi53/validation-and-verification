using BDJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Services
{
    public class TrainService
    {
    

        //public TrainService(TrainSystemContext context)
        //{
        //    _trainSystemContext = context;
        //}

        public static Train? searchTrainByDestination(string destination)
        {
            TrainSystemContext _trainSystemContext = new TrainSystemContext();
            var train = _trainSystemContext.Trains.OrderBy(train => train.DepartureDate)
                .First(train => train.DestinationStation.Equals(destination));
            return train;
        }

        //public static Train? searchTrainByDepartureDate(DateTime date)
        //{
           
        //    var train = _trainSystemContext.Trains.First(train => train.DestinationStation.Equals(date));
        //    return train;
        //}
    }
}
