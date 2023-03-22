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


        public User? AddUser(string name, int age, DiscountCard? card)
        {
            var user = new User { Name = name, Age = age, Card = card };
            _trainSystemContext.Users.Add(user);
            _trainSystemContext.SaveChanges();
            return user;
        }

        public User? SearchUserById(int id)
        {
            return _trainSystemContext.Users.FirstOrDefault(u => u.Id == id);
        }

    }
}
