using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DAL.MockData;

namespace DAL
{
    public class UserAccess : IUserAcess
    {

        public IEnumerable<User> LoginUser(string userName, string password)
        {
            var users = MockData.Instance.GetUsersInfo();

            return users;
        }
    }
}
