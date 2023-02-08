using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUserAcess
    {
        public IEnumerable<User> LoginUser(string userName);
        public List<User> ReadUsersData();
        public void UserToFile(User user);
    }
}
