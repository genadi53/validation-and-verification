using BDJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDJ.Services
{
    public class UserService
    {
        private readonly TrainSystemContext _trainSystemContext;

        public UserService(TrainSystemContext trainSystemContext)
        {
            _trainSystemContext = trainSystemContext;
        }


        public void addUser(string name, int age, DiscountCard? card)
        {
            _trainSystemContext.Users.Add(new User { Name = name, Age = age, Card = card });
            _trainSystemContext.SaveChanges();
        }

    }
}
